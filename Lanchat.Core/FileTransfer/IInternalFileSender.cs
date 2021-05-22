namespace Lanchat.Core.FileTransfer
{
    internal interface IInternalFileSender
    {
        void SendFile();
        void HandleReject();
        void HandleError();
    }
}