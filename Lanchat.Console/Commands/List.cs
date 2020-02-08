using Lanchat.Console.Ui;
using Lanchat.Common.Types;
using System.Collections.Generic;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void List()
        {
            var nodes = new List<string>();
            var count = 0;

            foreach (var item in program.Network.NodeList)
            {
                nodes.Add($"{item.Nickname} ({GetStatus(item.State)})");
                count++;
            }

            Prompt.Out("");
            Prompt.Out($"Connected peers: {count}");

            foreach (var item in nodes)
            {
                Prompt.Out(item);
            }
        }

        private string GetStatus(Status status)
        {
            if (status == Status.Ready)
            {
                return "\u001b[92monline\u001b[0m";
            }
            else
            {
                return "\u001b[91offline\u001b[0m"; 
            }
        }
    }
}