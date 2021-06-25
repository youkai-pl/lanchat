using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class DetectedUsersView : SimpleControl
    {
        public DetectedUsersView()
        {
            Content = new TextBlock
            {
                Text = "Users detected in network"
            };
        }
    }
}