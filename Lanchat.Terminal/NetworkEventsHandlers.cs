using Lanchat.Common.NetworkLib;
using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.Types;
using Lanchat.Terminal.Ui;
using System;
using System.Globalization;
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
            if (e != null)
            {
                Prompt.Log.Add($"Host started on port {e.Port}", Prompt.OutputType.Clear);
                Prompt.Port.Text = e.Port.ToString(CultureInfo.CurrentCulture);
                Prompt.Nodes.Text = network.NodeList.Count.ToString(CultureInfo.CurrentCulture);
                Prompt.Status.Text = "#broadcast";
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }

        public void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            if (e != null)
            {
                if (e.Target == MessageTarget.Private)
                {
                    Prompt.Log.Add(e.Content.Trim(), Prompt.OutputType.PrivateMessage, $"{e.Node.Nickname}");
                }
                else
                {
                    Prompt.Log.Add(e.Content.Trim(), Prompt.OutputType.Message, e.Node.Nickname);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }

        public void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
            if (e != null)
            {
                Prompt.Log.Add($"{e.Node.Nickname} connected", Prompt.OutputType.Notify);
                Prompt.Nodes.Text = network.NodeList.Count.ToString(CultureInfo.CurrentCulture);
                if (config.Muted.Exists(x => x == e.Node.Ip.ToString()))
                {
                    var node = network.NodeList.Find(x => x.Ip.Equals(e.Node.Ip));
                    node.Mute = true;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }

        public void OnNodeDisconnected(object o, NodeConnectionStatusEventArgs e)
        {
            if (e != null)
            {
                Prompt.Log.Add($"{e.Node.Nickname} disconnected", Prompt.OutputType.Notify);
                Prompt.Nodes.Text = network.NodeList.Count.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }

        public void OnNodeSuspended(object o, NodeConnectionStatusEventArgs e)
        {
            if (e != null)
            {
                Prompt.Log.Add($"{e.Node.Nickname} suspended. Waiting for reconnect", Prompt.OutputType.Notify);
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }

        public void OnNodeResumed(object o, NodeConnectionStatusEventArgs e)
        {
            if (e != null)
            {
                Prompt.Log.Add($"{e.Node.Nickname} reconnected", Prompt.OutputType.Notify);
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }

        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            if (e != null)
            {
                Prompt.Log.Add($"{e.OldNickname} changed nickname to {e.NewNickname}", Prompt.OutputType.Notify);
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }
    }
}
