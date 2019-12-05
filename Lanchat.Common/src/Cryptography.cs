using System.Security.Cryptography;
using System.Xml.Linq;

namespace Lanchat.Common.CryptographyLib
{
    public class Cryptography
    {
        public Cryptography()
        {
            csp = new RSACryptoServiceProvider(1024);
        }

        private readonly RSACryptoServiceProvider csp;
        public string PublicKey
        {
            get
            {
                return XElement.Parse(csp.ToXmlString(false)).Element("Modulus").Value;
            }
        }
    }
}