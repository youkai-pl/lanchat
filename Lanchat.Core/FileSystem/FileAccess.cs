using System;
using System.IO;
using System.Linq;

namespace Lanchat.Core.FileSystem
{
    internal class FileAccess : IFileAccess
    {
        private readonly string path;
        private byte[] buffer;
        private FileStream fileStream;
        private int bytesRead;

        public bool EndReached { get; private set; }

        internal FileAccess(string path)
        {
            this.path = path;
        }

        public byte[] ReadChunk(int chunkSize)
        {
            buffer ??= new byte[chunkSize];
            fileStream ??= File.OpenRead(path);
            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
            if (bytesRead <= 0)
            {
                EndReached = true;
            }

            return buffer.Take(bytesRead).ToArray();
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