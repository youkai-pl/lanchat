using System.Collections.Generic;
using System.Diagnostics;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.FileTransfer
{
    public class FileTransferHandler : IApiHandler
    {
        private readonly FileReceiver fileReceiver;
        private readonly FileSender fileSender;

        internal FileTransferHandler(FileReceiver fileReceiver, FileSender fileSender)
        {
            this.fileReceiver = fileReceiver;
            this.fileSender = fileSender;
        }

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[] {DataTypes.FileTransferRequest};

        public void Handle(DataTypes type, object data)
        {
            var request = (FileTransferStatus)data;
            HandleFileExchangeRequest(request);
        }

        private void HandleFileExchangeRequest(FileTransferStatus request)
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