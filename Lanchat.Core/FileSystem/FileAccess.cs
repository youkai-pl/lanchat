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
        
        internal FileAccess(string path)
        {
            this.path = path;
        }

        public bool ReadChunk(int chunkSize, out byte[] chunk)
        {
            buffer ??= new byte[chunkSize];
            fileStream ??= File.OpenRead(path);
            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
           
            if (bytesRead <= 0)
            {
                chunk = null;
                return false;
            }

            chunk = buffer.Take(bytesRead).ToArray();
            return true;
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