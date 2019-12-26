namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    /// Network API outputs class.
    /// </summary>
    public class Output
    {
        // Constructor
        internal Output(Network network)
        {
            this.network = network;
        }

        // Fields
        private readonly Network network;

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