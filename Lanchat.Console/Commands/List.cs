using Lanchat.Common.NetworkLib;
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

        private string GetStatus(Node item)
        {
            if (item.State == Status.Ready)
            {
                if (item.Mute)
                {
                    return "\u001b[93mmuted\u001b[0m";
                }
                else
                {
                    return "\u001b[92monline\u001b[0m";
                }
            }
            else
            {
                return "\u001b[91offline\u001b[0m";
            }
        }
    }
}