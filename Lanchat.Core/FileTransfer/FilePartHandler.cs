using System;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.FileTransfer
{
    internal class FilePartHandler : ApiHandler<FilePart>
    {
        private readonly FileReceiver fileReceiver;

        public FilePartHandler(FileReceiver fileReceiver)
        {
            this.fileReceiver = fileReceiver;
        }

        protected override void Handle(FilePart filePart)
        {
            if (fileReceiver.Request == null) return;
            if (!fileReceiver.Request.Accepted) return;

            try
            {
                var data = fileReceiver.Encryption.Decrypt(filePart.Data);
                fileReceiver.WriteFileStream.Write(data, 0, data.Length);
                fileReceiver.Request.PartsTransferred++;
                if (!filePart.Last) return;
                fileReceiver.OnFileTransferFinished(fileReceiver.Request);
                fileReceiver.WriteFileStream.Dispose();
                fileReceiver.Request = null;
            }
            catch (Exception e)
            {
                fileReceiver.CancelReceive();
                fileReceiver.OnFileTransferError();
            }
        }
    }
}