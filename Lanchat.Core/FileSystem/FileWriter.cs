using System;
using System.IO;

namespace Lanchat.Core.FileSystem
{
    internal class FileWriter : IDisposable
    {
        private readonly string path;
        private FileStream fileStream;

        internal FileWriter(string path)
        {
            this.path = path;
        }

        public void WriteChunk(byte[] chunk)
        {
            fileStream ??= File.OpenWrite(path);
            fileStream.Write(chunk);
        }

        public void Dispose()
        {
            fileStream?.Dispose();
        }
    }
}