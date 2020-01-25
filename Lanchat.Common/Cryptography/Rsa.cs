using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Lanchat.Common.Cryptography
{
    internal class Rsa
    {
        private readonly RSACryptoServiceProvider csp;

        internal Rsa()
        {
            csp = new RSACryptoServiceProvider(1024);
        }

        internal string PublicKey
        {
            get
            {
                return JsonConvert.SerializeObject(csp.ExportParameters(false));
            }
        }

        // RSA encode with specific key
        internal static string Encode(string input, string key)
        {
            var rsa = new RSACryptoServiceProvider();
            var rsaKeyInfo = JsonConvert.DeserializeObject<RSAParameters>(key);
            rsa.ImportParameters(rsaKeyInfo);
            var encryptedOutput = rsa.Encrypt(Encoding.UTF8.GetBytes(input), false);
            return Convert.ToBase64String(encryptedOutput);
        }

        // RSA decode with self key
        internal string Decode(string input)
        {
            byte[] clearOutput = csp.Decrypt(Convert.FromBase64String(input), false);
            return Encoding.UTF8.GetString(clearOutput);
        }
    }
}