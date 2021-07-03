using System;

namespace Lanchat.Core.Encryption
{
    /// <summary>
    ///     Events related to encryption.
    /// </summary>
    public interface IEncryptionAlerts
    {
        /// <summary>
        ///     Received public key of first connected node.
        /// </summary>
        event EventHandler<string> NewKey;
        
        /// <summary>
        ///     Received public key other than saved in PEM files.
        /// </summary>
        event EventHandler<string> ChangedKey;
    }
}