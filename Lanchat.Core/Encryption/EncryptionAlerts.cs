using System;

namespace Lanchat.Core.Encryption
{
    /// <inheritdoc cref="Lanchat.Core.Encryption.IEncryptionAlerts" />
    public class EncryptionAlerts : IEncryptionAlerts, IInternalEncryptionAlerts
    {
        /// <inheritdoc />
        public event EventHandler<string> NewKey;

        /// <inheritdoc />
        public event EventHandler<string> ChangedKey;

        /// <inheritdoc />
        public void OnNewKey(string e)
        {
            NewKey?.Invoke(this, e);
        }

        /// <inheritdoc />
        public void OnChangedKey(string e)
        {
            ChangedKey?.Invoke(this, e);
        }
    }
}