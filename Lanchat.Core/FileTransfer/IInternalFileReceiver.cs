using Lanchat.Core.FileSystem;

namespace Lanchat.Core.FileTransfer
{
    internal interface IInternalFileReceiver
    {
        CurrentFileTransfer CurrentFileTransfer { get; set; }
        FileWriter FileWriter { get; }
        void OnFileTransferError();
        void OnFileTransferRequestReceived();
        void FinishReceive();
        void HandleError();
        void CancelReceive(bool deleteFile);
    }
}