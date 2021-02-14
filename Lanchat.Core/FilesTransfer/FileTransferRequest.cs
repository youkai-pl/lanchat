using System.IO;

namespace Lanchat.Core.FilesTransfer
{
    public class FileTransferRequest
    {
        public bool Accepted { get; internal set; }
        public string FilePath { get; internal init; }
        public string FileName => Path.GetFileName(FilePath);
        public long Parts { get; internal init; }
        public long PartsTransferred { get; internal set; }
        public long Progress => 100 * PartsTransferred / Parts;
    }
}