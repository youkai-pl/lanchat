namespace Lanchat.Core.Encryption
{
    internal interface IInternalEncryptionAlerts
    {
        void OnNewKey(string e);
        void OnChangedKey(string e);
    }
}