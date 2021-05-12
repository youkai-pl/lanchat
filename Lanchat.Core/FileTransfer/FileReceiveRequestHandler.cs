using Lanchat.Core.Api;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileReceiveRequestHandler : ApiHandler<FileReceiveRequest>
    {
        private readonly FileReceiver fileReceiver;
        private readonly IStorage storage;

        internal FileReceiveRequestHandler (FileReceiver fileReceiver, IStorage storage)
        {
            this.fileReceiver = fileReceiver;
            this.storage = storage;
        }
        
        protected override void Handle(FileReceiveRequest data)
        {
            if (fileReceiver.CurrentFileTransfer is {Disposed: false})
            {
                return;
            }

            fileReceiver.CurrentFileTransfer = new CurrentFileTransfer
            {
                FilePath = storage.GetFilePath(data.FileName),
                Parts = data.PartsCount
            };
            
            fileReceiver.OnFileTransferRequestReceived();
        }
    }
}