// Lanchat 2
// Bartłomiej Tota 2019
// MIT License

using System.Threading;

namespace lanchat
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // show welcome screen
            Prompt.Welcome();

            // check config
            if (string.IsNullOrEmpty(Properties.User.Default.nick))
            {
                string nick = Prompt.Query("Choose nickname: ");
                while (nick.Length > 20)
                {
                    Prompt.Alert("Max 20 charcters");
                    nick = Prompt.Query("Choose nickname: ");
                }
                Properties.User.Default.nick = nick;
                Properties.User.Default.Save();
            }

            // initialize prompt
            new Thread(Prompt.Init).Start();
        }
    }
}