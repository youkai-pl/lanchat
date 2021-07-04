using System;
using System.Security.Cryptography;
using Lanchat.Core.Config;

namespace Lanchat.Core.Encryption
{
    internal class LocalRsa : ILocalRsa
    {
        public LocalRsa(IRsaDatabase rsaDatabase)
        {
            try
            {
                Rsa = RSA.Create();
                Rsa.ImportFromPem(rsaDatabase.GetLocalPem());
            }
            catch (ArgumentException)
            {
                Rsa = RSA.Create(2048);
                var pemFile = PemEncoding.Write("RSA PRIVATE KEY", Rsa.ExportRSAPrivateKey());
                rsaDatabase.SaveLocalPem(new string(pemFile));
            }
        }

        public RSA Rsa { get; }
    }
}