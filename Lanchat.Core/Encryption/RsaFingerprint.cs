using System.Linq;
using System.Security.Cryptography;

namespace Lanchat.Core.Encryption
{
    /// <summary>
    ///     RSA fingerprint generator
    /// </summary>
    public static class RsaFingerprint
    {
        /// <summary>
        ///     Get MD5 based fingerprint.
        /// </summary>
        /// <param name="key">RSA key</param>
        /// <returns>MD5 fingerprint</returns>
        public static string GetMd5(byte[] key)
        {
            var hash = MD5.HashData(key);
            var fingerprint = string.Concat(hash.Select(b => b.ToString("x2").Insert(2, ":")));
            return fingerprint.Remove(fingerprint.Length - 1);
        }
    }
}