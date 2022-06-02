using System;
using PomogatorAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Google.Cloud.Firestore;
using static PomogatorAPI.Config;
using System.Security.Cryptography;
using PomogatorAPI.Interfaces;

namespace PomogatorAPI.Repositories
{
	public class AuthService : IAuth
	{
		
		private readonly IConfiguration _config;
		public AuthService(IConfiguration config)
		{
			_config = config;
		}

		private readonly FirestoreDb db = FirestoreDb.Create(fbProjectId);

		private const string fbCollection = "customer_login";

		public LoginData User { get ; set; } = new LoginData();

		public String AuthCode { get; set; } = string.Empty;

		public string Token { get; set; } = string.Empty;

		private void CreateToken(LoginData user, string role)
        {
			List<Claim> claims = new List<Claim>()
            {
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Role, role)
            };

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
				_config.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token  = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(14),
				signingCredentials: creds
				);

			Token = new JwtSecurityTokenHandler().WriteToken(token);
        }

		private async Task FindUser(string id)
        {
			DocumentReference docRef = db.Collection(fbCollection).Document(id);
			DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

			if (!snapshot.Exists)
				throw new ArgumentException();

			User = snapshot.ConvertTo<LoginData>();
			
		}

		public async Task SignIn(string id, string authCode, string role)
        {
			await CodeVerification(id, authCode);

			await DeleteUserLogin(id);

			CreateToken(User, role);
        }

		private bool CompareAuthCodeHash(string authCode, byte[] codeHash, byte[] codeSalt)
        {
			using (var hmac = new HMACSHA512(codeSalt))
            {
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(authCode));
				return computedHash.SequenceEqual(codeHash);
            }
        }

		public async Task CodeVerification(string id, string authCode)
        {
			await FindUser(id);

			if (!CompareAuthCodeHash(authCode, User.CodeHash, User.CodeSalt))
				throw new ArgumentException();

		}

		public void GenerateCode(string id)
        {
			Random generator = new Random();
			AuthCode = generator.Next(0, 1000000).ToString("D4") + id;
		}

		public async Task PostUserLogin(LoginData user)
        {
			DocumentReference docRef = db.Collection(fbCollection).Document(user.Id);
			Dictionary<string, object> _user = new Dictionary<string, object>(){
					{"Id", user.Id},
					{"TelNum", user.TelNum},					
					{"CodeHash", user.CodeHash},
					{"CodeSalt", user.CodeSalt},
				};
			await docRef.SetAsync(_user);
		}

		public async Task UpdateUserLogin(LoginData user)
		{
			DocumentReference docRef = db.Collection(fbCollection).Document(user.Id);
			Dictionary<string, object> _user = new Dictionary<string, object>(){
					{"Id", user.Id},
					{"TelNum", user.TelNum},
					{"CodeHash", user.CodeHash},
					{"CodeSalt", user.CodeSalt},
				};
			await docRef.UpdateAsync(_user);
		}

		public async Task DeleteUserLogin(string id)
        {
			if (DoesUserLoginExistAsync(id).Result)
			{
				DocumentReference customerRef = db.Collection(fbCollection).Document(id);
				DocumentSnapshot snapshot = await customerRef.GetSnapshotAsync();
				Customer customer = snapshot.ConvertTo<Customer>();
				await customerRef.DeleteAsync();
			}
			else
				throw new ArgumentException();
		}

		private async Task<bool> DoesUserLoginExistAsync(string id)
		{
			DocumentReference docRef = db.Collection(fbCollection).Document(id);
			DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

			if (snapshot.Exists)
				return true;

			return false;
		}

		private void CreateCodeHash(string code, out byte[] codeHash, out byte[] codeSalt)
        {
			using(var hmac = new HMACSHA512())
            {
				codeSalt = hmac.Key;
				codeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(code));
            }
        }

		public async Task GetAuthCodeAsync(string id, string telNum)
        {
			GenerateCode(id);

			CreateCodeHash(AuthCode, out byte[] codeHash, out byte[] codeSalt);

			User = new LoginData(id, telNum, codeHash, codeSalt);
			
			if(DoesUserLoginExistAsync(id).Result)
				await UpdateUserLogin(User);
			else
				await PostUserLogin(User);

        }

		public static string GetUserId(ClaimsIdentity identity)
        {
			IEnumerable<Claim> claims = identity.Claims;
			return claims.Where(p => p.Type == "NameIdentifier").FirstOrDefault().Value;
		}

	}
}

