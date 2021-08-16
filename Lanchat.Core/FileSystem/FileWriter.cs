using System;
using System.IO;

namespace Lanchat.Core.FileSystem
{
    internal class FileWriter : IDisposable
    {
        private readonly FileStream fileStream;

        internal FileWriter(string path)
        {
            fileStream = new FileStream(path, FileMode.Append);
        }

        public void Dispose()
        {
            fileStream?.Dispose();
        }

        public void WriteChunk(byte[] chunk)
        {
            fileStream.Write(chunk, 0, chunk.Length);
        }
    }
}