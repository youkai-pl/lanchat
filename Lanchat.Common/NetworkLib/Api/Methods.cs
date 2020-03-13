using Lanchat.Common.NetworkLib.Node;
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
        /// Manual connect.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="port">Node host port</param>
        public void Connect(IPAddress ip, int port)
        {
            network.CreateNode(ip, port, true);
        }

        /// <summary>
        /// Get node by IP.
        /// </summary>
        /// <param name="ip">IP address</param>
        /// <returns></returns>
        public NodeInstance GetNode(IPAddress ip)
        {
            return network.NodeList.Find(x => x.Ip.Equals(ip));
        }

        /// <summary>
        /// Get node by nickname.
        /// </summary>
        /// <param name="nickname">Nickname</param>
        /// <returns></returns>
        public NodeInstance GetNode(string nickname)
        {
            return network.NodeList.Find(x => x.Nickname == nickname);
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