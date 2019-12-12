namespace Lanchat.Common.NetworkLib
{
    public class ApiMethods
    {
        // Constructor
        public ApiMethods(Network network)
        {
            this.network = network;
        }

        private readonly Network network;

        // Send message to all nodes
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

        // Change nickname
        public void ChangeNickname(string nickname)
        {
            network.NodeList.ForEach(x =>
            {
                if (x.Client != null)
                {
                    x.Client.SendNickname(nickname);
                }
            });
        }

        // Send random data to all nodes
        public void DestroyLanchat()
        {
            network.NodeList.ForEach(x =>
            {
                if (x.Client != null)
                {
                    x.Client.DestroyLanchat();
                }
            });
        }
    }
}