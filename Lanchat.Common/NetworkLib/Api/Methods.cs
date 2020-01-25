namespace Lanchat.Common.NetworkLib.Api
{
    /// <summary>
    /// Network API outputs class.
    /// </summary>
    public class Methods
    {
        private readonly Network network;

        internal Methods(Network network)
        {
            this.network = network;
        }

        /// <summary>
        /// Send message to all nodes.
        /// </summary>
        /// <param name="message">content</param>
        public void SendAll(string message)
        {
            network.NodeList.ForEach(x =>
            {
                if (x.Client != null)
                {
                    x.Client.SendMessage(message);
                }
            });
        }
    }
}