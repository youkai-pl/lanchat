using Lanchat.Core.Api;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer.Handlers
{
    internal class FileReceiveRequestHandler : ApiHandler<FileReceiveRequest>
    {
        private readonly IInternalFileReceiver fileReceiver;
        private readonly IStorage storage;

        public FileReceiveRequestHandler(IInternalFileReceiver fileReceiver, IStorage storage)
        {
            this.fileReceiver = fileReceiver;
            this.storage = storage;
        }

        protected override void Handle(FileReceiveRequest data)
        {
            if (fileReceiver.CurrentFileTransfer is { Disposed: false })
            {
                return;
            }

            if (data.PartsCount < 1)
            {
                return;
            }

            fileReceiver.CurrentFileTransfer = new CurrentFileTransfer
            {
                FilePath = storage.GetNewFilePath(data.FileName),
                Parts = data.PartsCount
            };

            fileReceiver.OnFileTransferRequestReceived();
        }
    }
}