using System;
using System.IO;
using System.Security.Cryptography;

namespace Lanchat.Common.Cryptography
{
    internal class Aes
    {
        private readonly AesManaged aes;

        // Create instance with random key
        internal Aes()
        {
            aes = new AesManaged();
            aes.GenerateKey();
            aes.GenerateIV();
        }

        // Create instance with speciefed key
        internal Aes(string key, string iv)
        {
            aes = new AesManaged
            {
                Key = Convert.FromBase64String(key),
                IV = Convert.FromBase64String(iv)
            };
        }

        internal string IV
        {
            get
            {
                return Convert.ToBase64String(aes.IV);
            }
        }
        internal string Key
        {
            get
            {
                return Convert.ToBase64String(aes.Key);
            }
        }

        internal string Decode(string input)
        {
            string plaintext = null;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(input)))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                        plaintext = reader.ReadToEnd();
                }
            }
            return plaintext;
        }

        internal string Encode(string input)
        {
            byte[] encrypted;
            ICryptoTransform encryptor = aes.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                        sw.Write(input);
                    encrypted = ms.ToArray();
                }
            }
            return Convert.ToBase64String(encrypted);
        }
    }
}