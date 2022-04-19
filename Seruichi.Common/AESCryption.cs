using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Seruichi.Common
{
    public class AESCryption
    {
        public static readonly string DefaultKey = "10_4_Ld";
        public static readonly string DefaultKey2 = "1433";

        private const int STRETCHING_TIMES = 1000;
        private const int KEY_SIZE = 256;
        private const int BLOCK_SIZE = 128;
        private const int SALT_SIZE = 25;
        private const int IV_SIZE = BLOCK_SIZE / 8;

        private readonly Encoding encoding = Encoding.GetEncoding("Shift_JIS");

        public AESCryption()
        {
        }

        private byte[] CreateSalt()
        {
            return GenerateRandomData(SALT_SIZE);
        }

        private byte[] CreateAesIV()
        {
            return GenerateRandomData(IV_SIZE);
        }

        private byte[] CreateAesKey(string keyName, byte[] salt)
        {
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(keyName, salt);
            deriveBytes.IterationCount = STRETCHING_TIMES;
            return deriveBytes.GetBytes(KEY_SIZE / 8);
        }

        private byte[] GenerateRandomData(int length)
        {
            var rnd = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(rnd);
            }
            return rnd;
        }

        public string GenerateRandomDataBase64(int length)
        {
            return Convert.ToBase64String(GenerateRandomData(length));
        }

        public string EncryptToBase64(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(key))
            {
                return "";
            }

            byte[] salt = CreateSalt();
            byte[] aesKey = CreateAesKey(key, salt);
            byte[] aesIv = CreateAesIV();

            byte[] byteValue = encoding.GetBytes(plainText);
            int byteLength = byteValue.Length;

            using (var aes = CreateAesCng(aesKey, aesIv))
            using (var encryptor = aes.CreateEncryptor())
            {
                byte[] encryptedValue = encryptor.TransformFinalBlock(byteValue, 0, byteLength);
                byte[] byteData = new byte[salt.Length + aesIv.Length + encryptedValue.Length];

                int len1 = Buffer.ByteLength(salt);
                int len2 = Buffer.ByteLength(aesIv);
                int len3 = Buffer.ByteLength(encryptedValue);

                Buffer.BlockCopy(salt, 0, byteData, 0, len1);
                Buffer.BlockCopy(aesIv, 0, byteData, len1, len2);
                Buffer.BlockCopy(encryptedValue, 0, byteData, len1 + len2, len3);
                return Convert.ToBase64String(byteData);
            }
        }

        public string DecryptFromBase64(string encryptedValue, string key)
        {
            if (string.IsNullOrEmpty(encryptedValue) || string.IsNullOrEmpty(key))
            {
                return encryptedValue;
            }

            try
            {
                byte[] byteData = Convert.FromBase64String(encryptedValue);
                byte[] salt = byteData.Take(SALT_SIZE).ToArray();
                byte[] aesIv = byteData.Skip(SALT_SIZE).Take(IV_SIZE).ToArray();
                byte[] byteEncrypted = byteData.Skip(SALT_SIZE + IV_SIZE).ToArray();
                byte[] aesKey = CreateAesKey(key, salt);

                using (var aes = CreateAesCng(aesKey, aesIv))
                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedValue = decryptor.TransformFinalBlock(byteEncrypted, 0, byteEncrypted.Length);
                    return encoding.GetString(decryptedValue);
                }
            }
            catch
            {
                return encryptedValue;
            }
        }

        private AesCng CreateAesCng(byte[] aesKey, byte[] aesIv)
        {
            var aes = new AesCng();
            aes.KeySize = KEY_SIZE;
            aes.BlockSize = BLOCK_SIZE;
            aes.Mode = CipherMode.CBC;
            aes.Key = aesKey;
            aes.IV = aesIv;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }
    }
}
