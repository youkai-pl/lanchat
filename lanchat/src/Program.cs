// lanchat 2
// tofu 2019

using System;
using System.Threading;

namespace lanchat
{
    class Program
    {
        static void Main(string[] args)
        {
            // show welcome screen
            Prompt.Welcome();

            // initialize prompt
            var promptThread = new Thread(Prompt.Read);
            promptThread.Start();

            // check config
            if (string.IsNullOrEmpty(Properties.User.Default.nick))
            {
                Properties.User.Default.nick = "default";
                Properties.User.Default.filesPath = "default";
                Properties.User.Default.Save();
            }

            Thread.Sleep(2000);
            Prompt.Notice("notice");
            Thread.Sleep(1000);
            Prompt.Alert("alert");
            Thread.Sleep(600);
            Prompt.Message("test", "dasdasdsdasdsad");
        }
    }
}
