using System;

namespace Lanchat.Common.NetworkLib
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

    /// <summary>
    /// Cannot establish connection.
    /// </summary>
    public class ConnectionFailedException : Exception
    {
        internal ConnectionFailedException()
        {
        }

        internal ConnectionFailedException(string message) : base(message)
        {
        }

        internal ConnectionFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}