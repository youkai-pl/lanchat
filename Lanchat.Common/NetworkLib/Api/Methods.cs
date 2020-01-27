using System.Net;

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

        /// <summary>
        /// Manual connect.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="port">Node host port</param>
        public void Connect(IPAddress ip, int port)
        {
            network.CreateNode(new Node(port, ip), true);
        }
    }
}