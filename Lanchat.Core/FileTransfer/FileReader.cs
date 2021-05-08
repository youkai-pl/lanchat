using System.IO;
using System.Linq;
// ReSharper disable InvertIf

namespace Lanchat.Core.FileTransfer
{
    internal class FileReader
    {
        private readonly CurrentFileTransfer currentFileTransfer;
        private readonly byte[] buffer;
        private int bytesRead;
        
        internal bool EndReached { get; private set; }
        
        internal FileReader(CurrentFileTransfer currentFileTransfer, int chunkSize)
        {
            this.currentFileTransfer = currentFileTransfer;
            currentFileTransfer.FileStream = File.OpenRead(currentFileTransfer.FilePath);
            buffer = new byte[chunkSize];
        }
        
        internal byte[] ReadChunk()
        {
            bytesRead = currentFileTransfer.FileStream.Read(buffer, 0, buffer.Length);
            
            if (bytesRead <= 0)
            {
                EndReached = true;
            }
            
            return buffer.Take(bytesRead).ToArray();
        }
    }
}