using System;
using System.Security.Cryptography;
using System.Text;

namespace Seruichi.Common
{
    public class PasswordHash
    {
        const int STRETCHING_TIMES = 10000;

        private readonly Encoding encoding = Encoding.UTF8;

        private byte[] GenerateSalt(string username)
        {
            var data = encoding.GetBytes(username.ToLower());
            var sha256 = new SHA256CryptoServiceProvider();
            var hash = sha256.ComputeHash(data);
            return hash;
            //var saltString = String.Concat(username.ToLower(), StaticCache.GetDataCryptionKey());
            //using (var csp = new SHA256CryptoServiceProvider())
            //{
            //    return csp.ComputeHash(encoding.GetBytes(saltString));
            //}
        }

        public string GeneratePasswordHash(string username, string password)
        {
            var salt = this.GenerateSalt(username);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, STRETCHING_TIMES);

            //byte[] salt = this.GenerateSalt(username);
            //var pbkdf2 = new Rfc2898DeriveBytes(password, salt, STRETCHING_TIMES, HashAlgorithmName.SHA256);

            string result = Convert.ToBase64String(pbkdf2.GetBytes(32));
            return result;
        }
    }
}
