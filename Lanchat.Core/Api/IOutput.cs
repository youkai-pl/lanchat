namespace Lanchat.Core.Api
{
    /// <summary>
    ///     Sending data to node.
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        ///     Send data.
        /// </summary>
        /// <param name="content">Object to send.</param>
        void SendData(object content);

        /// <summary>
        ///     Send the data even if the connection process is not finished yet.
        /// </summary>
        /// <param name="content">Model object.</param>
        void SendPrivilegedData(object content);
    }
}