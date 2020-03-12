using System;

namespace Lanchat.Common.NetworkLib.Exceptions
{
    /// <summary>
    /// Node already exist in list.
    /// </summary>
    public class NodeAlreadyExistException : Exception
    {
        internal NodeAlreadyExistException()
        {
        }

        internal NodeAlreadyExistException(string message) : base(message)
        {
        }

        internal NodeAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
