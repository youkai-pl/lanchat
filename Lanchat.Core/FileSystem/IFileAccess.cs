using System;

namespace Lanchat.Core.FileSystem
{
    internal interface IFileAccess : IDisposable
    {
        bool EndReached { get; }
        byte[] ReadChunk(int chunkSize);
        void WriteChunk(byte[] chunk);
    }
}