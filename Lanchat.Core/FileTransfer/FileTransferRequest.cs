using System.IO;

namespace Lanchat.Core.FileTransfer
{
    public class FileTransferRequest
    {
        internal bool Accepted { get; set; }
        public string FilePath { get; internal init; }
        public string FileName => Path.GetFileName(FilePath);
        public long Parts { get; internal init; }
        public long PartsTransferred { get; internal set; }
        public long Progress => 100 * PartsTransferred / Parts;
    }
}