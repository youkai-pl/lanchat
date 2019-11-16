using System.Security.Cryptography;
using System.Xml.Linq;

namespace Lanchat.Common.Cryptography
{
    public static class Cryptography
    {
        public static RSACryptoServiceProvider csp;

        public static string Generate()
        {
            csp = new RSACryptoServiceProvider(1024);
            return csp.ToXmlString(true);
        }

        public static void Load(string config)
        {
            csp = new RSACryptoServiceProvider();
            csp.FromXmlString(config);
        }

        public static string GetPublic()
        {
            return XElement.Parse(csp.ToXmlString(false)).Element("Modulus").Value;
        }
    }
}