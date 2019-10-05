// Lanchat 2
// Bartłomiej Tota 2019

using lanchat.Crypto;
using lanchat.Network;
using lanchat.Terminal;
using System.Threading;

namespace lanchat
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // show welcome screen
            Prompt.Welcome();

            // check nick
            Prompt.Notice("Validating config");
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

            // try to load rsa settings
            Prompt.Notice("Validating RSA keys");
            try
            {
                Cryptography.Load();
            }
            catch
            {
                Prompt.Notice("Generating RSA keys");
                Properties.User.Default.csp = Cryptography.Generate();
                Properties.User.Default.Save();
            }

            // initialize prompt
            new Thread(Prompt.Init).Start();

            // initialize network
            Client.Init(Properties.User.Default.bport,
                        Properties.User.Default.nick,
                        Cryptography.GetPublic());
        }
    }
}