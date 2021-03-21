using System;

namespace Lanchat.Core.Encryption
{
    internal class InvalidKeyImportException : Exception
    {
        internal InvalidKeyImportException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}