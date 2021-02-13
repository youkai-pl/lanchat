using System.IO;

namespace Lanchat.Core.FilesTransfer
{
    public class FileTransferRequest
    {
        public string FilePath { get; internal set; }
        public string FileName => Path.GetFileName(FilePath);
        public bool Accepted { get; set; }
    }
}