using Lanchat.Common.NetworkLib.Node;
using Lanchat.Common.Types;
using Lanchat.Console.Ui;
using System.Collections.Generic;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void List()
        {
            var list = new List<string>();
            var count = 0;

            foreach (var item in program.Network.NodeList)
            {
                list.Add($"{item.Nickname} ({GetStatus(item)})");
                count++;
            }

            Prompt.Out("");
            Prompt.Out($"Connected peers: {count}");

            foreach (var item in list)
            {
                Prompt.Out(item);
            }
        }

        private static string GetStatus(NodeInstance item)
        {
            if (item.State == Status.Ready)
            {
                if (item.Mute)
                {
                    return "muted";
                }
                else
                {
                    return "online";
                }
            }
            else
            {
                return "offline";
            }
        }
    }
}