using System;
using System.Security.Cryptography;
using System.Text;

namespace Seruichi.Common
{
    public class PasswordHash
    {
        const int STRETCHING_TIMES = 10000;

        private readonly Encoding encoding = Encoding.UTF8;

        private string GenerateSalt(string username)
        {
            var data = encoding.GetBytes(username.ToLower());
            var sha256 = new SHA256CryptoServiceProvider();
            var hash = sha256.ComputeHash(data);

            string result = BitConverter.ToString(hash).ToLower().Replace("-", "");
            return result;
        }

        public string GeneratePasswordHash(string username, string password)
        {
            var salt = this.GenerateSalt(username);
            var pbkdf2 = new Rfc2898DeriveBytes(password, encoding.GetBytes(salt), STRETCHING_TIMES);

            string result = Convert.ToBase64String(pbkdf2.GetBytes(32));
            return result;
        }
    }
}
