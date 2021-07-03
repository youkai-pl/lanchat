using System;

namespace Lanchat.Core.Encryption
{
    public interface IEncryptionAlerts
    {
        event EventHandler<string> NewKey;
        event EventHandler<string> ChangedKey;
    }
}