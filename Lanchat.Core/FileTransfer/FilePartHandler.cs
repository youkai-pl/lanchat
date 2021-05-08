using System;
using System.Diagnostics;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FilePartHandler : ApiHandler<FilePart>
    {
        private readonly FileReceiver fileReceiver;
        private readonly IFileSystem fileSystem;

        internal FilePartHandler(FileReceiver fileReceiver, IFileSystem fileSystem)
        {
            this.fileReceiver = fileReceiver;
            this.fileSystem = fileSystem;
        }

        protected override void Handle(FilePart filePart)
        {
            if (fileReceiver.CurrentFileTransfer == null ||
                !fileReceiver.CurrentFileTransfer.Accepted ||
                fileReceiver.CurrentFileTransfer.Disposed)
            {
                return;
            }
            
            try
            {
                SavePart(filePart);
            }
            catch (Exception e)
            {
               fileSystem.CatchFileSystemException(e, () =>
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
            fileReceiver.CurrentFileTransfer.FileAccess.WriteChunk(data);
            fileReceiver.CurrentFileTransfer.PartsTransferred++;
        }
    }
}