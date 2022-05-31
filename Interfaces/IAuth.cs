using System;
using PomogatorAPI.Models;
namespace PomogatorAPI.Interfaces
{
	public interface IAuth
	{
		string AuthCode { get; set; }
		LoginData User { get; set; }
		string Token { get; set; }
		Task GetAuthCodeAsync(string id, string telNum);
		Task CodeVerification(string id, string authCode);
		Task SignIn(string id, string authCode, string role);
	}
}

