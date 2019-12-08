using System;
using System.IO;
using System.Security.Cryptography;

namespace Lanchat.Common.CryptographyLib
{
    // AES key generate
    public class AesInstance
    {
        // Self AES constructor
        public AesInstance()
        {
            aes = new AesManaged();
            aes.GenerateKey();
            aes.GenerateIV();
        }

        // AES constructor with parameters
        public AesInstance(string key, string iv)
        {
            aes = new AesManaged
            {
                Key = Convert.FromBase64String(key),
                IV = Convert.FromBase64String(iv)
            };
        }

        // Fields
        private readonly AesManaged aes;

        // Properties
        public string Key { get { return Convert.ToBase64String(aes.Key); } }

        public string IV { get { return Convert.ToBase64String(aes.IV); } }

        // Encode string
        public string Encode(string input)
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

        // Decode string
        public string Decode(string input)
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
    }
}