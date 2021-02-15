using System.Text.Json;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class FileExchangeRequestHandler : IApiHandler
    {
        private readonly FileTransferHandler fileTransferHandler;

        public FileExchangeRequestHandler( FileTransferHandler fileTransferHandler)
        {
            this.fileTransferHandler = fileTransferHandler;
        }
        
        public DataTypes DataType { get; } = DataTypes.FileExchangeRequest;
        public void Handle(object data)
        {
            var request = JsonSerializer.Deserialize<FileTransferStatus>((string) data, CoreConfig.JsonSerializerOptions);
            fileTransferHandler.HandleFileExchangeRequest(request);
        }
    }
}