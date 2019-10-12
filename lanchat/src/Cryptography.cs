using System.Security.Cryptography;
using System.Xml.Linq;
using static lanchat.Program;

namespace lanchat.Crypto
{
    public static class Cryptography
    {
        public static RSACryptoServiceProvider csp;

        public static string Generate()
        {
            csp = new RSACryptoServiceProvider(1024);
            return csp.ToXmlString(true);
        }

        public static void Load()
        {
            csp = new RSACryptoServiceProvider();
            csp.FromXmlString(Config["csp"]);
        }

        public static string GetPublic()
        {
            return XElement.Parse(csp.ToXmlString(false)).Element("Modulus").Value;
        }
    }
}