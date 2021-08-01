using System;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.Commands.General
{
    public class Exit : ICommand
    {
        public string Alias => "exit";
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] _)
        {
            Console.ResetColor();
            Console.Clear();
            Environment.Exit(0);
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}