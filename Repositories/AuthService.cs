using System;
using PomogatorAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Google.Cloud.Firestore;
using static PomogatorAPI.Config;
using System.Security.Cryptography;

namespace PomogatorAPI.Repositories
{
	public class AuthService
	{
		
		private readonly IConfiguration _config;
		public AuthService(IConfiguration config)
		{
			_config = config;
		}

		private readonly FirestoreDb db = FirestoreDb.Create(fbProjectId);

		private const string fbCollection = "customer_login";

		LoginData? User { get; set; }

		String? AuthCode { get; set; }

		private string CreateToken(LoginData user)
        {
			List<Claim> claims = new List<Claim>()
            {
				new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
				_config.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token  = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(14),
				signingCredentials: creds
				);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
        }

		private async Task/*<LoginData>*/ FindUser(string id)
        {
			DocumentReference docRef = db.Collection(fbCollection).Document(id);
			DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

			if (!snapshot.Exists)
				throw new ArgumentException();

			User = snapshot.ConvertTo<LoginData>();
			/*return loginData;*/
		}

		public async Task<string> SignIn(string id, string authCode)
        {

            await FindUser(id);
			/*LoginData userData = FindUser(id).Result;*/
			if (VerifyAuthCodeHash(authCode, User.CodeHash, User.CodeSalt))
               {
				var response = CreateToken(User);
				return response;
               }
			throw new ArgumentException();

        }

		private bool VerifyAuthCodeHash(string authCode, byte[] codeHash, byte[] codeSalt)
        {
			using (var hmac = new HMACSHA512(codeSalt))
            {
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(authCode));
				return computedHash.SequenceEqual(codeHash);
            }
        }

		public void GenerateCode()
        {
			Random generator = new Random();
			AuthCode = generator.Next(0, 1000000).ToString("D6");
		}

		public async Task PostUserLogin(LoginData user)
        {
			DocumentReference docRef = db.Collection(fbCollection).Document(user.Id);
			Dictionary<string, object> _user = new Dictionary<string, object>(){
					{"Id", user.Id},
					{"CodeHash", user.CodeHash},
					{"CodeSalt", user.CodeSalt },
				};
			await docRef.SetAsync(_user);
		}

		public async Task UpdateUserLogin(LoginData user)
		{
			DocumentReference docRef = db.Collection(fbCollection).Document(user.Id);
			Dictionary<string, object> _user = new Dictionary<string, object>(){
					{"Id", user.Id},
					{"CodeHash", user.CodeHash},
					{"CodeSalt", user.CodeSalt },
				};
			await docRef.UpdateAsync(_user);
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

		public async Task GetAuthCode(string id)
        {
			GenerateCode();

			CreateCodeHash(AuthCode, out byte[] codeHash, out byte[] codeSalt);

			User = new LoginData(id, codeHash, codeSalt);
			
			if(DoesUserLoginExistAsync(id).Result)
				await UpdateUserLogin(User);
			else
				await PostUserLogin(User);

        }

		public Task<string> SignUp()
	}
}

