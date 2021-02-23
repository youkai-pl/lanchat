using System;

namespace Lanchat.Core.Encryption
{
    public class InvalidKeyImportException : Exception
    {
        public InvalidKeyImportException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}