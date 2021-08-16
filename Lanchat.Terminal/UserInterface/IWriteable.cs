using ConsoleGUI.Data;

namespace Lanchat.Terminal.UserInterface
{
    public interface IWriteable
    {
        void AddText(string text, Color color);
    }
}