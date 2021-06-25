using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class FileTransfersView : SimpleControl
    {
        public FileTransfersView()
        {
            Content = new TextBlock
            {
                Text = "File transfers progress"
            };
        }
    }
}