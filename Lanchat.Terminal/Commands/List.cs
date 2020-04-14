using Lanchat.Common.NetworkLib;
using Lanchat.Common.NetworkLib.Node;
using Lanchat.Common.Types;
using Lanchat.Terminal.Ui;
using System;
using System.Collections.Generic;

namespace Lanchat.Terminal.Commands
{
    public static class List
    {
        public static void Execute(Network network)
        {

            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            var list = new List<string>();
            var count = 0;

            foreach (var item in network.NodeList)
            {
                list.Add($"{item.Nickname} ({GetStatus(item)})");
                count++;
            }

            foreach (var item in list)
            {
                Prompt.Log.Add(item);
            }
        }

        private static string GetStatus(NodeInstance item)
        {
            if (item.State == Status.Ready)
            {
                if (item.Mute)
                {
                    return Properties.Resources.StatusMuted;
                }
                else
                {
                    return Properties.Resources.StatusOnline;
                }
            }
            else
            {
                return Properties.Resources.StatusOffline;
            }
        }
    }
}
