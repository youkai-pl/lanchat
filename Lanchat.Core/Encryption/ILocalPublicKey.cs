using System.Security.Cryptography;

namespace Lanchat.Core.Encryption
{
    /// <summary>
    ///     Local RSA algorithm.
    /// </summary>
    public interface ILocalPublicKey
    {
        /// <summary>
        ///     RSA class.
        /// </summary>
        RSA LocalRsa { get; }

        /// <summary>
        ///     Get public key in PEM format.
        /// </summary>
        /// <returns>PEM string</returns>
        string GetPublicPem();
    }
}