using System;
using PomogatorAPI.Models;
namespace PomogatorAPI.Interfaces
{
	public interface ICustomerAuth
	{
		Task<List<string>> SignUpAsync(LoginData loginData);
		Task<List<string>> SignInAsync(LoginData loginData);
	}
}

