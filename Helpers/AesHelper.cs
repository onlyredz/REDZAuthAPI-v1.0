using System.Security.Cryptography;
using System.Text;

namespace REDZAuthApi.Helpers
{
    public static class AesHelper
    {
        private static string? _key;

        public static void Initialize(string key)
        {
            if (key.Length != 32)
                throw new ArgumentException("Encryption key must be exactly 32 characters long");
            _key = key;
        }

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(_key))
                throw new InvalidOperationException("AesHelper not initialized. Call Initialize() first.");

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = Convert.ToBase64String(aes.IV.Concat(encryptedBytes).ToArray());
            return result;
        }

        public static string Decrypt(string encrypted)
        {
            if (string.IsNullOrEmpty(_key))
                throw new InvalidOperationException("AesHelper not initialized. Call Initialize() first.");

            var fullBytes = Convert.FromBase64String(encrypted);
            var iv = fullBytes.Take(16).ToArray();
            var cipherBytes = fullBytes.Skip(16).ToArray();

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
