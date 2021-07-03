using System;

namespace Lanchat.Core.Encryption
{
    public class EncryptionAlerts : IEncryptionAlerts, IInternalEncryptionAlerts
    {
        public event EventHandler<string> NewKey;
        public event EventHandler<string> ChangedKey;

        public void OnNewKey(string e)
        {
            NewKey?.Invoke(this, e);
        }

        public void OnChangedKey(string e)
        {
            ChangedKey?.Invoke(this, e);
        }
    }
}