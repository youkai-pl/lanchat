using System;
using System.Collections.Generic;
using System.IO;

namespace Lanchat.Core.FileTransfer
{
    internal class FileReader : IDisposable
    {
        private readonly FileStream fileStream;
        private readonly byte[] buffer;

        internal int BytesRead { get; private set; }
        
        internal FileReader(string filePath, int chunkSize)
        {
            fileStream = File.OpenRead(filePath);
            buffer = new byte[chunkSize];
        }

        internal IEnumerable<byte> ReadChunk()
        {
            BytesRead = fileStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        
        public void Dispose()
        {
            fileStream?.Dispose();
        }
    }
}