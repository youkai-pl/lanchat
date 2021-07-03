using System;
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
    }
}