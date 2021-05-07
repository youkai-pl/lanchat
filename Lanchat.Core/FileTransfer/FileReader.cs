using System.IO;
using System.Linq;
// ReSharper disable InvertIf

namespace Lanchat.Core.FileTransfer
{
    internal class FileReader
    {
        private readonly byte[] buffer;
        private readonly FileStream fileStream;
        private int bytesRead;
        
        internal bool EndReached { get; private set; }
        
        internal FileReader(string filePath, int chunkSize)
        {
            fileStream = File.OpenRead(filePath);
            buffer = new byte[chunkSize];
        }
        
        internal byte[] ReadChunk()
        {
            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
            
            if (bytesRead <= 0)
            {
                EndReached = true;
                fileStream.Dispose();
            }
            
            return buffer.Take(bytesRead).ToArray();
        }
    }
}