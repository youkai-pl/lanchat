using System.Threading;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;
using Lanchat.Core.Extensions;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class DetectedUsersView : SimpleControl, IScrollable
    {
        public DetectedUsersView()
        {
            var detectedNodes = Program.Network.NodesDetection.DetectedNodes;
            var stackPanel = new VerticalStackPanel();

            ScrollPanel = new VerticalScrollPanel
            {
                Content = stackPanel,
                ScrollBarBackground = new Character(),
                ScrollBarForeground = new Character(),
            };
            
            Content = ScrollPanel;

            detectedNodes.CollectionChanged += (_, _) =>
            {
                stackPanel.Children.ForEach(x => stackPanel.Remove(x));
                detectedNodes.ForEach(x =>
                {
                    stackPanel.Add(new TextBlock
                    {
                        Text = $"{x.Nickname} - {x.IpAddress}"
                    });
                });
            };
        }

        public VerticalScrollPanel ScrollPanel { get; }
    }
}