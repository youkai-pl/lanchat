using System;
using System.IO;
using System.Security.Cryptography;

namespace Lanchat.Common.CryptographyLib
{
    // AES key generate
    public class AesInstance
    {
        // Constructor
        public AesInstance()
        {
            aes = new AesManaged();
            aes.GenerateKey();
            aes.GenerateIV();
        }

        // Fields
        private readonly AesManaged aes;

        // Properties
        public string Key { get { return Convert.ToBase64String(aes.Key); } }
        public string IV { get { return Convert.ToBase64String(aes.IV); } }


        // Encode string
        public string AesEncode(string input)
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
        public string AesDecode(string input)
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
