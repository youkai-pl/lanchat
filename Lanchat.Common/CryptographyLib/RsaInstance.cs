using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Lanchat.Common.CryptographyLib
{
    internal class RsaInstance
    {
        // RSA Provider
        private readonly RSACryptoServiceProvider csp;

        // Constructor
        internal RsaInstance()
        {
            csp = new RSACryptoServiceProvider(1024);
        }

        // Public key
        internal string PublicKey
        {
            get
            {
                return JsonConvert.SerializeObject(csp.ExportParameters(false));
            }
        }

        // RSA decode
        internal string Decode(string input)
        {
            byte[] clearOutput = csp.Decrypt(Convert.FromBase64String(input), false);
            return Encoding.UTF8.GetString(clearOutput);
        }
    }
}