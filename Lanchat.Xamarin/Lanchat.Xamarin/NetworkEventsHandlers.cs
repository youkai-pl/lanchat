using Lanchat.Common.NetworkLib;
using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.Types;
using System;
using System.Globalization;
namespace Lanchat.Xamarin
{
    public class NetworkEventsHandlers
    {
        private readonly Network network;
        private readonly MainPage mainPage;

        public NetworkEventsHandlers(MainPage mainPage, Network network)
        {
            this.mainPage = mainPage;
            this.network = network;
        }

        public void OnHostStarted(object o, HostStartedEventArgs e)
        {
            if (e != null)
            {
                mainPage.AddToLog($"Running on port {e.Port.ToString(CultureInfo.CurrentCulture)}");
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
                    mainPage.AddToLog($"-> {e.Node.Nickname}: {e.Content.Trim()}");
                }
                else
                {
                    mainPage.AddToLog($"{e.Node.Nickname}: {e.Content.Trim()}");
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
                mainPage.AddToLog($"{e.Node.Nickname} connected");
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
                mainPage.AddToLog($"{e.Node.Nickname} disconnected");
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
                mainPage.AddToLog($"{e.Node.Nickname} suspended. Waiting for reconnect");
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
                mainPage.AddToLog($"{e.Node.Nickname} reconnected");
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
                mainPage.AddToLog($"{e.OldNickname} changed nickname to {e.NewNickname}");
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }
    }
}
