using System;
using Google.Cloud.Firestore;
namespace PomogatorAPI.Models
{
	public class LoginData
	{
		[FirestoreProperty]
		public string Id { get; set; } = string.Empty;
		[FirestoreProperty]
		public byte[] CodeHash { get; set; }
		[FirestoreProperty]
		public byte[] CodeSalt { get; set; }

		public LoginData()
		{
		}
	}
}

