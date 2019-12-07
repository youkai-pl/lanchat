using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Lanchat.Common.CryptographyLib
{
    public class Cryptography
    {
        public Cryptography()
        {
            csp = new RSACryptoServiceProvider(1024);
        }

        // RSA Provider
        private readonly RSACryptoServiceProvider csp;

        // Public key
        public string PublicKey
        {
            get
            {
                return JsonConvert.SerializeObject(csp.ExportParameters(false));
            }
        }

        // RSA encode
        public static string AsymetricEncode(string input, string key)
        {
            var rsa = new RSACryptoServiceProvider();
            var rsaKeyInfo = JsonConvert.DeserializeObject<RSAParameters>(key);
            rsa.ImportParameters(rsaKeyInfo);
            var encryptedOutput = rsa.Encrypt(Encoding.UTF8.GetBytes(input), false);
            return Convert.ToBase64String(encryptedOutput);
        }

        // RSA decode
        public string AsymetricDecode(string input)
        {
            byte[] clearOutput = csp.Decrypt(Convert.FromBase64String(input), false);
            return Encoding.UTF8.GetString(clearOutput);
        }

        // AES key generate
        public static AesManaged CreateAesInstance()
        {
            AesManaged aes = new AesManaged();
            aes.GenerateKey();
            aes.GenerateIV();
            return aes;
        }

        // AES encode
        public static string AesEncode(AesManaged aes, string input)
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

        // AES decode
        public static string AesDecode(AesManaged aes, string input)
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