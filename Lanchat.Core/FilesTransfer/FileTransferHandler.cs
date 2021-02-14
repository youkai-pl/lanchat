using System.Diagnostics;
using Lanchat.Core.Models;

namespace Lanchat.Core.FilesTransfer
{
    public class FileTransferHandler
    {
        private readonly FileReceiver fileReceiver;
        private readonly FileSender fileSender;

        internal FileTransferHandler(FileReceiver fileReceiver, FileSender fileSender)
        {
            this.fileReceiver = fileReceiver;
            this.fileSender = fileSender;
        }

        internal void HandleFileExchangeRequest(FileTransferStatus request)
        {
            switch (request.RequestStatus)
            {
                case RequestStatus.Accepted:
                    fileSender.SendFile();
                    break;

                case RequestStatus.Rejected:
                    fileSender.HandleReject();
                    break;

                case RequestStatus.Sending:
                    fileReceiver.HandleReceiveRequest(request);
                    break;

                case RequestStatus.Errored:
                    fileReceiver.HandleSenderError();
                    break;

                case RequestStatus.Canceled:
                    fileSender.HandleCancel();
                    break;

                default:
                    Trace.Write("Node received file exchange request of unknown type.");
                    break;
            }
        }
    }
}