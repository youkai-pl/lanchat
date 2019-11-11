using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace lanchat.core.CryptographyLib
{
    public static class Cryptography
    {
        public static RSACryptoServiceProvider csp;

        public static string Generate()
        {
            csp = new RSACryptoServiceProvider(1024);
            return csp.ToXmlString(true);
        }

        public static void Load(IConfigurationRoot config)
        {
            csp = new RSACryptoServiceProvider();
            csp.FromXmlString(config["csp"]);
        }

        public static string GetPublic()
        {
            return XElement.Parse(csp.ToXmlString(false)).Element("Modulus").Value;
        }
    }
}