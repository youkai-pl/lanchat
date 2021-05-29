using System;
using System.IO;
using System.Linq;

namespace Lanchat.Core.FileSystem
{
    internal class FileReader : IDisposable
    {
        private readonly FileStream fileStream;
        private readonly byte[] buffer;
        
        internal FileReader(int chunkSize, string path)
        {
            buffer = new byte[chunkSize];
            fileStream = File.OpenRead(path);
        }

        public bool ReadChunk(out byte[] chunk)
        {
            var bytesRead = fileStream.Read(buffer);
            chunk = buffer.Take(bytesRead).ToArray();
            return bytesRead > 0;
        }

        public void Dispose()
        {
            fileStream?.Dispose();
        }
    }
}