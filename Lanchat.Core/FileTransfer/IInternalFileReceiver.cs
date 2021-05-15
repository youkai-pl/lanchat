using Lanchat.Core.FileSystem;

namespace Lanchat.Core.FileTransfer
{
    internal interface IInternalFileReceiver : IFileReceiver
    {
        FileWriter FileWriter { get; }
        void OnFileTransferError();
        void OnFileTransferRequestReceived();
        void FinishReceive();
        void HandleError();
    }
}