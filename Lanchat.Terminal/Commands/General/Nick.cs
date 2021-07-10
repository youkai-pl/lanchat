namespace Lanchat.Terminal.Commands.General
{
    public class Nick : ICommand
    {
        public string Alias => "nick";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            var nickname = args[0].Trim();
            if (nickname.Length >= 20 || string.IsNullOrWhiteSpace(nickname))
            {
            }
            else
            {
                Program.Config.Nickname = nickname;
            }
        }
    }
}