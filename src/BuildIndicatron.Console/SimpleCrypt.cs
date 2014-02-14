using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BuildIndicatron.Console
{
    public class SimpleCrypt
    {
        // Change these keys
        private readonly ICryptoTransform _decryptorTransform;
        private readonly ICryptoTransform _encryptorTransform;

        private static readonly byte[] _key = {
            175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209, 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24,
        };

        private static readonly byte[] _vector = { 113, 119, 231, 121, 221, 112, 79, 32, 114, 156, 146, 64, 191, 111, 23, 3, };

        public SimpleCrypt()
            : this(_key, _vector)
        {
        }

        public SimpleCrypt(byte[] key, byte[] vector)
        {
            var rm = new RijndaelManaged();
            _encryptorTransform = rm.CreateEncryptor(key, vector);
            _decryptorTransform = rm.CreateDecryptor(key, vector);
        }

        #region Static

        public static byte[] GenerateEncryptionKey()
        {
            var rm = new RijndaelManaged();
            rm.GenerateKey();
            return rm.Key;
        }

        public static byte[] GenerateEncryptionVector()
        {
            var rm = new RijndaelManaged();
            rm.GenerateIV();
            return rm.IV;
        }

        #endregion

        public string Encrypt(string textValue)
        {
            var bytes = Encoding.UTF8.GetBytes(textValue);
            byte[] encrypted;
            using (var memoryStream = new MemoryStream())
            {
                using (var cs = new CryptoStream(memoryStream, _encryptorTransform, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    encrypted = StreamToBytes(memoryStream);
                    cs.Close();
                }

            }
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string encryptedValue)
        {

            byte[] bytes = Convert.FromBase64String(encryptedValue);
            Byte[] decryptedBytes;
            using (var encryptedStream = new MemoryStream())
            {
                using (var decryptStream = new CryptoStream(encryptedStream, _decryptorTransform, CryptoStreamMode.Write))
                {
                    decryptStream.Write(bytes, 0, bytes.Length);
                    decryptStream.FlushFinalBlock();
                    decryptedBytes = StreamToBytes(encryptedStream);
                }

            }
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        #region Private Methods

        private static byte[] StreamToBytes(MemoryStream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            var encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);
            return encrypted;
        }

        #endregion
    }
}