using System.Security.Cryptography;

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
            csp.FromXmlString(lanchat.Properties.User.Default.csp);
        }
    }
}