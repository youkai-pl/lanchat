// Lanchat 2
// Bartłomiej Tota 2019 
// MIT License

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
                Properties.User.Default.nick = "default";
                Properties.User.Default.filesPath = "default";
                Properties.User.Default.Save();
            }

            // initialize prompt
            new Thread(Prompt.Init).Start();
        }
    }
}
