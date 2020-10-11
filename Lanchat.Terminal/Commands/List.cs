using System.Collections.Generic;
using Lanchat.Core;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal.Commands
{
    public static class List
    {
        public static void Execute(P2P network)
        {
            var list = new List<string>();
            network.Nodes.ForEach(x => list.Add($"{x.Nickname}"));

            foreach (var item in list)
            {
                Prompt.Log.Add(item);
            }
        }
    }
}