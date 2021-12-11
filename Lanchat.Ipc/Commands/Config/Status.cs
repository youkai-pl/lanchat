using Lanchat.Core.Identity;

namespace Lanchat.Ipc.Commands.Config
{
    public class Status : ICommand
    {
        public string Alias => "status";
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            switch (args[0])
            {
                case "online":
                    Program.Config.UserStatus = UserStatus.Online;
                    break;

                case "afk":
                    Program.Config.UserStatus = UserStatus.AwayFromKeyboard;
                    break;

                case "dnd":
                    Program.Config.UserStatus = UserStatus.DoNotDisturb;
                    break;

                default:
                    Program.IpcSocket.Send("invalid_argument");
                    break;
            }
        }
    }
}