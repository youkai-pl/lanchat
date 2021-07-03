using System.Net;
using Lanchat.Core.Config;

namespace Lanchat.ClientCore
{
    /// <inheritdoc />
    public class RsaDatabase : IRsaDatabase
    {
        /// <inheritdoc />
        public string GetLocalPem()
        {
            return Storage.ReadPemFile("localhost");
        }

        /// <inheritdoc />
        public void SaveLocalPem(string pem)
        {
            Storage.SavePemFile("localhost", pem);
        }

        /// <inheritdoc />
        public string GetNodePem(IPAddress ipAddress)
        {
            return Storage.ReadPemFile(ipAddress.ToString());
        }

        /// <inheritdoc />
        public void SaveNodePem(IPAddress ipAddress, string pem)
        {
            Storage.SavePemFile(ipAddress.ToString(), pem);
        }
    }
}