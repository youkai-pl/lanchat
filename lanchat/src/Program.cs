// lanchat 2
// tofu 2019

using System.Threading;

namespace lanchat
{
    class Program
    {
        static void Main(string[] args)
        {
            // Welcome screen
            Prompt.Welcome();

            //Check config
            if (string.IsNullOrEmpty(Properties.User.Default.nick))
            {
                Prompt.Notice("First configuration");
                Properties.User.Default.nick = Prompt.Query("Nickname", false);
                Properties.User.Default.Save();
            }

            // Initialize prompt
            var ts = new ThreadStart(Prompt.Read);
            var backgroundThread = new Thread(ts);
            backgroundThread.Start();
        }
    }
}
