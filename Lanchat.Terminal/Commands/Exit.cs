using System;

namespace Lanchat.Terminal.Commands
{
    public static class Exit
    {
        public static void Execute()
        {
            Console.ResetColor();
            Console.Clear();
            Environment.Exit(0);
        }
    }
}