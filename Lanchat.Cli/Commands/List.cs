using Lanchat.Cli.Ui;
using Lanchat.Common.Types;
using System.Collections.Generic;

namespace Lanchat.Cli.Commands
{
    public partial class Command
    {
        public void List()
        {
            var nodes = new List<string>();
            var count = 0;

            foreach (var item in program.Network.NodeList)
            {
                nodes.Add($"{item.Nickname} ({item.Ip})");
                count++;
            }

            Prompt.Out($"Connected peers: {count}");

            foreach (var item in nodes)
            {
                Prompt.Out(item);
            }
        }
    }
}