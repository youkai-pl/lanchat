using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.Types;
using Lanchat.Xamarin.ViewModels;
using System;
using System.Globalization;
namespace Lanchat.Xamarin
{
    public class NetworkEventsHandlers
    {
        private readonly MainViewModel mainPage;

        public NetworkEventsHandlers(MainViewModel mainPage)
        {
            this.mainPage = mainPage;
        }

        public void OnHostStarted(object o, HostStartedEventArgs e)
        {
            if (e != null)
            {
                mainPage.AddMessage($"Running on port {e.Port.ToString(CultureInfo.CurrentCulture)}");
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
                    mainPage.AddMessage($"-> {e.Node.Nickname}: {e.Content.Trim()}");
                }
                else
                {
                    mainPage.AddMessage($"{e.Node.Nickname}: {e.Content.Trim()}");
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
                mainPage.AddMessage($"{e.Node.Nickname} connected");
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
                mainPage.AddMessage($"{e.Node.Nickname} disconnected");
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
                mainPage.AddMessage($"{e.Node.Nickname} suspended. Waiting for reconnect");
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
                mainPage.AddMessage($"{e.Node.Nickname} reconnected");
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
                mainPage.AddMessage($"{e.OldNickname} changed nickname to {e.NewNickname}");
            }
            else
            {
                throw new ArgumentNullException(nameof(e));
            }
        }
    }
}
