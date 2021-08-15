using System;
using System.Diagnostics;
using Lanchat.Core.Api;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer.Handlers
{
    internal class FilePartHandler : ApiHandler<FilePart>
    {
        private readonly IInternalFileReceiver fileReceiver;
        private readonly IStorage storage;

        public FilePartHandler(IInternalFileReceiver fileReceiver, IStorage storage)
        {
            this.fileReceiver = fileReceiver;
            this.storage = storage;
        }

        protected override void Handle(FilePart filePart)
        {
            if (fileReceiver.CurrentFileTransfer is not { Accepted: true } || 
                fileReceiver.CurrentFileTransfer.Disposed||
                fileReceiver.CurrentFileTransfer.Parts < fileReceiver.CurrentFileTransfer.PartsTransferred)
            {
                return;
            }

            try
            {
                SavePart(filePart);
            }
            catch (Exception e)
            {
                storage.CatchFileSystemException(e, () =>
                {
                    fileReceiver.CancelReceive(false);
                    fileReceiver.OnFileTransferError();
                    Trace.WriteLine("Cannot access file system");
                });
            }
        }

        private void SavePart(FilePart filePart)
        {
            var base64Data = filePart.Data;
            var data = Convert.FromBase64String(base64Data);
            fileReceiver.FileWriter.WriteChunk(data);
            fileReceiver.CurrentFileTransfer.PartsTransferred++;
        }
    }
}