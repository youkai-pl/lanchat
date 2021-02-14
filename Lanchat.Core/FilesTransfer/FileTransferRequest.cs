using System.IO;

namespace Lanchat.Core.FilesTransfer
{
    public class FileTransferRequest
    {
        public string FilePath { get; internal set; }
        public string FileName => Path.GetFileName(FilePath);
        public bool Accepted { get; set; }
        public long Parts { get; set; }
        public long PartsTransferred { get; set; }
        public long Progress => 100 * PartsTransferred / Parts;
    }
}