using ConsoleGUI.Controls;

namespace Lanchat.Terminal.UserInterface
{
    public interface IScrollable
    {
        VerticalScrollPanel ScrollPanel { get; set; }
    }
}