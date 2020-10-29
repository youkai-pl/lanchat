using System;

namespace Lanchat.Terminal.Commands
{
    public static class Exit
    {
        public static void Execute()
        {
            Console.Clear();
            Console.ResetColor();
            Environment.Exit(0);
        }
    }
}