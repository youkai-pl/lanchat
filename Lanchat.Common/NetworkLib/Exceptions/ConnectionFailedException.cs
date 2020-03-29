using System;

namespace Lanchat.Common.NetworkLib.Exceptions
{
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