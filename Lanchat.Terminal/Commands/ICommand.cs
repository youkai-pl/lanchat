using Lanchat.Core.Network;

namespace Lanchat.Terminal.Commands
{
    public interface ICommand
    {
        string[] Aliases { get; }
        int ArgsCount { get; }
        int ContextArgsCount { get; }
        void Execute(string[] args);
        void Execute(string[] args, INode node);
    }
}