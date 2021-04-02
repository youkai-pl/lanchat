namespace Lanchat.Core.API
{
    /// <summary>
    ///     Send data other of type not belonging to standard Lanchat.Core set.
    /// </summary>
    public interface INetworkOutput
    {
        /// <summary>
        ///     Send data.
        /// </summary>
        /// <param name="content">Object to send.</param>
        void SendData(object content);
        
        /// <summary>
        ///     Send the data before marking the node as ready (Handshake, KeyInfo...).
        /// </summary>
        /// <param name="content">Object to send.</param>
        void SendPrivilegedData(object content);
    }
}