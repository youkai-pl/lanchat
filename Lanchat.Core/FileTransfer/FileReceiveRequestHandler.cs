using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileReceiveRequestHandler : ApiHandler<FileReceiveRequest>
    {
        private readonly FileReceiver fileReceiver;
        private readonly IFileSystem fileSystem;

        internal FileReceiveRequestHandler (FileReceiver fileReceiver, IFileSystem fileSystem)
        {
            this.fileReceiver = fileReceiver;
            this.fileSystem = fileSystem;
        }
        
        protected override void Handle(FileReceiveRequest data)
        {
            if (fileReceiver.Request != null)
            {
                return;
            }

            fileReceiver.Request = new CurrentFileTransfer
            {
                FilePath = fileSystem.GetFilePath(data.FileName),
                Parts = data.PartsCount
            };
            
            fileReceiver.OnFileTransferRequestReceived();
        }
    }
}