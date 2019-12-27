using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Lanchat.Common.Cryptography
{
    internal static class Rsa
    {
        // RSA encode
        internal static string Encode(string input, string key)
        {
            var rsa = new RSACryptoServiceProvider();
            var rsaKeyInfo = JsonConvert.DeserializeObject<RSAParameters>(key);
            rsa.ImportParameters(rsaKeyInfo);
            var encryptedOutput = rsa.Encrypt(Encoding.UTF8.GetBytes(input), false);
            return Convert.ToBase64String(encryptedOutput);
        }
    }
}