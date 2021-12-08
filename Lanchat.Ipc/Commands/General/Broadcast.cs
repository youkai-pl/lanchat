namespace Lanchat.Ipc.Commands.General
{
    public class Broadcast : ICommand
    {
        public string Alias => "broadcast";
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            Program.Network.Broadcast.SendMessage(string.Join(" ", args));
        }
    }
}