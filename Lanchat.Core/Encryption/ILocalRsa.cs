using System.Security.Cryptography;

namespace Lanchat.Core.Encryption
{
    /// <summary>
    ///     Local RSA algorithm.
    /// </summary>
    public interface ILocalRsa
    {
        /// <summary>
        ///     RSA class.
        /// </summary>
        RSA Rsa { get; }
    }
}