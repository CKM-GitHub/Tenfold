﻿using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Seruichi.Common
{
    public class AESCryption
    {
        public static readonly string DefaultKey = "10_4_Ld";

        private const int keySize = 256;
        private const int blockSize = 128;
        private const int saltSize = 25;
        private const int ivSize = blockSize / 8;
        private readonly Encoding encoding = Encoding.GetEncoding("ISO-2022-JP");

        public AESCryption()
        {
        }

        private byte[] CreateSalt()
        {
            return GenerateRandomData(saltSize);
        }

        private byte[] CreateAesIV()
        {
            return GenerateRandomData(ivSize);
        }

        private byte[] CreateAesKey(string keyName, byte[] salt)
        {
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(keyName, salt);
            deriveBytes.IterationCount = 1000;
            return deriveBytes.GetBytes(keySize / 8);
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

        public string EncryptToBase64(string plainText, string key)
        {
            byte[] salt = CreateSalt();
            byte[] aesKey = CreateAesKey(key, salt);
            byte[] aesIv = CreateAesIV();

            //var saltString = Convert.ToBase64String(salt);
            //var aesKeyString = Convert.ToBase64String(aesKey);
            //var aesIvString = Convert.ToBase64String(aesIv);

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
            byte[] byteData = Convert.FromBase64String(encryptedValue);
            byte[] salt = byteData.Take(saltSize).ToArray();
            byte[] aesIv = byteData.Skip(saltSize).Take(ivSize).ToArray();
            byte[] byteEncrypted = byteData.Skip(saltSize + ivSize).ToArray();
            byte[] aesKey = CreateAesKey(key, salt);

            //var saltString = Convert.ToBase64String(salt);
            //var aesKeyString = Convert.ToBase64String(aesKey);
            //var aesIvString = Convert.ToBase64String(aesIv);

            using (var aes = CreateAesCng(aesKey, aesIv))
            using (var decryptor = aes.CreateDecryptor())
            {
                byte[] decryptedValue = decryptor.TransformFinalBlock(byteEncrypted, 0, byteEncrypted.Length);
                return encoding.GetString(decryptedValue);
            }
        }

        private AesCng CreateAesCng(byte[] aesKey, byte[] aesIv)
        {
            var aes = new AesCng();
            aes.KeySize = keySize;
            aes.BlockSize = blockSize;
            aes.Mode = CipherMode.CBC;
            aes.Key = aesKey;
            aes.IV = aesIv;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }
    }
}
