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

