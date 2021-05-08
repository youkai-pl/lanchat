using System;

namespace Lanchat.Core.FileSystem
{
    internal interface IFileAccess : IDisposable
    {
        bool ReadChunk(int chunkSize, out byte[] chunk);
        void WriteChunk(byte[] chunk);
    }
}