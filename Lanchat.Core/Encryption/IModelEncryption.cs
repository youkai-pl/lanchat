namespace Lanchat.Core.Encryption
{
    public interface IModelEncryption
    {
        void EncryptObject(object data);
        void DecryptObject(object data);
    }
}