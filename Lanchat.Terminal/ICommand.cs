namespace Lanchat.Terminal
{
    public interface ICommand
    {
        public void Execute(string[] args);
        public string Alias { get; set; }
        public int ArgsCount { get; set; }
    }
}