using Lanchat.Core.Api;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer
{
    public class FileTransferOutput
    {
        private readonly IOutput output;

        public FileTransferOutput(IOutput output)
        {
            this.output = output;
        }

        internal void SendSignal(FileTransferStatus status)
        {
            output.SendData(new FileTransferControl
            {
                Status = status
            });
        }
        
        internal void SendRequest(CurrentFileTransfer request)
        {
            output.SendData(new FileReceiveRequest
            {
                FileName = request.FileName,
                PartsCount = request.Parts
            });
        }

        internal void SendPart(FilePart filePart)
        {
            output.SendData(filePart);
        }
    }
}