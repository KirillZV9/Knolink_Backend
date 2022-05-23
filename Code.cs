using System;
using System.Text;



/// <summary>
/// Хэширование кода + ключ 
/// </summary>
/// 
public class Code
{
    public void GenerateHash(out byte[] CodeHash, out byte[] CodeSalt)
    {
        Random rnd = new Random();
        int code = rnd.Next(100000, 999999);
        string Code = Convert.ToString(code);

        using (var hash = new System.Security.Cryptography.HMACSHA512())
        {
            CodeHash = hash.ComputeHash(Encoding.UTF8.GetBytes(Code));
            CodeSalt = hash.Key;
        }
    }

}