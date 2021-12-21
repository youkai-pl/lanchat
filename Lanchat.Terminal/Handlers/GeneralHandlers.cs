using System.Collections.Specialized;
using Lanchat.Core.Network;
using Lanchat.Core.NodesDiscovery;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Handlers
{
    public class GeneralHandlers
    {
        public GeneralHandlers(IP2P network)
        {
            network.NodesDetection.ReceivedLists.CollectionChanged += NodesListReceived;
        }

        private void NodesListReceived(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (NodesList list in e.NewItems)
            {
                Writer.WriteWarning($"{list.Sender.User.Nickname} sent you nodes list with following addresses:");
                list.Addresses.ForEach(x => Writer.WriteWarning(x.ToString()));
                Writer.WriteWarning("If you trust this user use /yes command to connect them");
            }
        }
    }
}