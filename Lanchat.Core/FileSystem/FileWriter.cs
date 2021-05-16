using System;
using System.IO;

namespace Lanchat.Core.FileSystem
{
    internal class FileWriter : IDisposable
    {
        private readonly FileStream fileStream;

        internal FileWriter(string path)
        {
            fileStream = File.OpenWrite(path);
        }

        public void WriteChunk(byte[] chunk)
        {
            fileStream.Write(chunk, 0, chunk.Length);
        }

        public void Dispose()
        {
            fileStream?.Dispose();
        }
    }
}