using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileTransferSignalling
    {
        private readonly IOutput output;

        internal FileTransferSignalling(IOutput output)
        {
            this.output = output;
        }

        internal void SignalAccept()
        {
            output.SendData(new FileTransferControl
            {
                FileTransferSignal = FileTransferSignal.Accepted
            });
        }

        internal void SignalReject()
        {
            output.SendData(new FileTransferControl
            {
                FileTransferSignal = FileTransferSignal.Rejected
            });
        }

        internal void SignalCancel()
        {
            output.SendData(new FileTransferControl
            {
                FileTransferSignal = FileTransferSignal.Canceled
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

        internal void SignalFinished()
        {
            output.SendData(new FileTransferControl
            {
                FileTransferSignal = FileTransferSignal.Finished
            });
        }

        internal void SignalErrored()
        {
            output.SendData(new FileTransferControl
            {
                FileTransferSignal = FileTransferSignal.Errored
            });
        }
    }
}