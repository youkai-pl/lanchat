using System.Diagnostics;
using Lanchat.Core.Api;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer.Handlers
{
    internal class FileTransferControlHandler : ApiHandler<FileTransferControl>
    {
        private readonly IInternalFileReceiver fileReceiver;
        private readonly IInternalFileSender fileSender;

        public FileTransferControlHandler(IInternalFileReceiver fileReceiver, IInternalFileSender fileSender)
        {
            this.fileReceiver = fileReceiver;
            this.fileSender = fileSender;
        }

        protected override void Handle(FileTransferControl request)
        {
            switch (request.Status)
            {
                case FileTransferStatus.Accepted:
                    fileSender.SendFile();
                    break;

                case FileTransferStatus.Rejected:
                    fileSender.HandleReject();
                    break;

                case FileTransferStatus.Finished:
                    fileReceiver.FinishReceive();
                    break;

                case FileTransferStatus.ReceiverError:
                    fileSender.HandleError();
                    break;

                case FileTransferStatus.SenderError:
                    fileReceiver.HandleError();
                    break;

                case FileTransferStatus.ReceiveCancelled:
                    fileSender.HandleError();
                    break;
                
                case FileTransferStatus.SendCancelled:
                    fileReceiver.HandleError();
                    break;
                
                default:
                    Trace.Write("Node received file exchange request of unknown type.");
                    break;
            }
        }
    }
}