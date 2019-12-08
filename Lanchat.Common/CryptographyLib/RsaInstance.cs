using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Lanchat.Common.CryptographyLib
{
    public class RsaInstance
    {
        // Constructor
        public RsaInstance()
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

        // RSA decode
        public string AsymetricDecode(string input)
        {
            byte[] clearOutput = csp.Decrypt(Convert.FromBase64String(input), false);
            return Encoding.UTF8.GetString(clearOutput);
        }
    }
}