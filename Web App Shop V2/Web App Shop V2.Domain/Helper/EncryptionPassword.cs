using System;
using System.Security.Cryptography;
using System.Text;

namespace Web_App_Shop_V2.Domain.Helper;

public class EncryptionPassword
{
	public static string HashPassword(string password) // метод кодировки пароля
	{
		using(var sha256 = SHA256.Create())
		{
			var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			var hash = BitConverter.ToString(hashedBytes).Replace(".", "").ToLower();

			return hash;
		}
	}
}

