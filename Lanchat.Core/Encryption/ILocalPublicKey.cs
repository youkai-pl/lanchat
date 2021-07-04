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
    }
}