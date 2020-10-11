using System;

namespace Lanchat.Terminal.Commands
{
    public static class ExitLanchat
    {
        public static void Execute()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
