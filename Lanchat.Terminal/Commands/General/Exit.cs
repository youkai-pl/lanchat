using System;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.Commands.General
{
    public class Exit : ICommand
    {
        public string[] Aliases { get; } =
        {
            "exit",
            "e"
        };
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] _)
        {
            Environment.Exit(0);
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}