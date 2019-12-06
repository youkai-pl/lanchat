using System;
using System.Diagnostics;
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
    }
}