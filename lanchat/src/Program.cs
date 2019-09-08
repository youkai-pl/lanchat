// lanchat 2
// tofu 2019

using System.Threading;

namespace lanchat
{
    class Program
    {
        static void Main(string[] args)
        {
            // show welcome screen
            Prompt.Welcome();

            // check config
            if (string.IsNullOrEmpty(Properties.User.Default.nick))
            {
                Prompt.Notice("First configuration");
                Properties.User.Default.nick = Prompt.Query("Nickname", false);
                Properties.User.Default.Save();
            }

            // initialize prompt
            var promptThread = new Thread(new ThreadStart(Prompt.Read));
            promptThread.Start();
        }
    }
}
