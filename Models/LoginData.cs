using System;
using Google.Cloud.Firestore;
namespace PomogatorAPI.Models
{
	[FirestoreData]
	public class LoginData
	{
		[FirestoreProperty]
		public string Id { get; set; } = string.Empty;
		[FirestoreProperty]
		public string TelNum { get; set; } = string.Empty; 
		[FirestoreProperty]
		public byte[] CodeHash { get; set; }
		[FirestoreProperty]
		public byte[] CodeSalt { get; set; }
		/*[FirestoreProperty]
		public string CodeHash
        {
            get
            {
				return System.Text.Encoding.UTF8.GetString(CodeHashBytes);
            }
            set
            {
				CodeHashBytes = System.Text.Encoding.UTF8.GetBytes(value);
            }
        }
		[FirestoreProperty]
		public string CodeSalt
		{
			get
			{
				return System.Text.Encoding.UTF8.GetString(CodeSaltBytes);
			}
			set
			{
				CodeSaltBytes = System.Text.Encoding.UTF8.GetBytes(value);
			}
		}*/

		public LoginData()
		{
		}

		public LoginData(string id, string telNum, byte[] codeHash, byte[] codeSalt)
        {
			Id = id;
			TelNum = telNum;
			CodeHash = codeHash;
		    CodeSalt = codeSalt;
        }
	}
}

