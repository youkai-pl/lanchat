using System.Text.Json;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class FilePartHandler : IApiHandler
    {
        private readonly FileReceiver fileReceiver;

        public FilePartHandler(FileReceiver fileReceiver)
        {
            this.fileReceiver = fileReceiver;
        }
        public DataTypes DataType { get; } = DataTypes.FilePart;

        public void Handle(object data)
        {
            var binary = JsonSerializer.Deserialize<FilePart>((string) data);
            fileReceiver.HandleReceivedFilePart(binary);
        }
    }
}