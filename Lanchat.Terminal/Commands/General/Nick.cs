using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.General
{
    public class Nick : ICommand
    {
        public string Alias => "nick";
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            var nickname = args[0].Trim();
            if (nickname.Length >= 20 || string.IsNullOrWhiteSpace(nickname))
            {
                Writer.WriteError(Resources._WrongNickname);
            }
            else
            {
                Program.Config.Nickname = nickname;
            }
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}