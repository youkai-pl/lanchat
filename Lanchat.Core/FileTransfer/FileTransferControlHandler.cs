using System.Diagnostics;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileTransferControlHandler : ApiHandler<FileTransferControl>
    {
        private readonly FileReceiver fileReceiver;
        private readonly FileSender fileSender;

        internal FileTransferControlHandler(FileReceiver fileReceiver, FileSender fileSender)
        {
            this.fileReceiver = fileReceiver;
            this.fileSender = fileSender;
        }

        protected override void Handle(FileTransferControl request)
        {
            switch (request.FileTransferSignal)
            {
                case FileTransferSignal.Accepted:
                    fileSender.SendFile();
                    break;

                case FileTransferSignal.Rejected:
                    fileSender.HandleReject();
                    break;

                case FileTransferSignal.Canceled:
                    fileSender.HandleCancel();
                    break;

                case FileTransferSignal.Finished:
                    fileReceiver.FinishReceive();
                    break;

                case FileTransferSignal.Errored:
                    fileReceiver.HandleSenderError();
                    break;

                default:
                    Trace.Write("Node received file exchange request of unknown type.");
                    break;
            }
        }
    }
}