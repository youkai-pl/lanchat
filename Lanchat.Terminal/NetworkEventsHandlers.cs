using Lanchat.Common.NetworkLib;
using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.Types;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal
{
    public class NetworkEventsHandlers
    {
        private readonly Config config;
        private readonly Network network;

        public NetworkEventsHandlers(Config config, Network network)
        {
            this.config = config;
            this.network = network;
        }

        public void OnHostStarted(object o, HostStartedEventArgs e)
        {
            Prompt.Log.Add($"Host started on port {e.Port}", Prompt.OutputType.Clear);
        }

        public void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            if (e.Target == MessageTarget.Private)
            {
                Prompt.Log.Add(e.Content.Trim(), Prompt.OutputType.Message, $"{e.Node.Nickname} -> {config.Nickname}");
            }
            else
            {
                Prompt.Log.Add(e.Content.Trim(), Prompt.OutputType.Message, e.Node.Nickname);
            }
        }

        public void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
            Prompt.Log.Add($"{e.Node.Nickname} connected", Prompt.OutputType.Notify);

            if (config.Muted.Exists(x => x == e.Node.Ip.ToString()))
            {
                var node = network.NodeList.Find(x => x.Ip.Equals(e.Node.Ip));
                node.Mute = true;
            }
        }

        public void OnNodeDisconnected(object o, NodeConnectionStatusEventArgs e)
        {
            Prompt.Log.Add($"{e.Node.Nickname} disconnected", Prompt.OutputType.Notify);
        }

        public void OnNodeSuspended(object o, NodeConnectionStatusEventArgs e)
        {
            Prompt.Log.Add($"{e.Node.Nickname} suspended. Waiting for reconnect", Prompt.OutputType.Notify);
        }

        public void OnNodeResumed(object o, NodeConnectionStatusEventArgs e)
        {
            Prompt.Log.Add($"{e.Node.Nickname} reconnected", Prompt.OutputType.Notify);
        }

        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            Prompt.Log.Add($"{e.OldNickname} changed nickname to {e.NewNickname}", Prompt.OutputType.Notify);
        }
    }
}
