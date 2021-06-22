using ConsoleGUI.Controls;
using ConsoleGUI.Data;

namespace Lanchat.Terminal.UserInterface
{
    public class HomeTab : Box
    {
        public HomeTab()
        {
            HorizontalContentPlacement = HorizontalPlacement.Center;
            VerticalContentPlacement = VerticalPlacement.Center;
            Content = new Border
            {
                BorderStyle = BorderStyle.Single,
                Content = new TextBlock
                {
                    Text = " Lanchat 2.8 Demo "
                }
            };
        }
    }
}