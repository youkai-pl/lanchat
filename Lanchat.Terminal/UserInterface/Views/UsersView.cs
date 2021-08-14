using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class UsersView : SimpleControl, IScrollable
    {
        private readonly VerticalStackPanel connectedPanel = new();
        private readonly VerticalStackPanel detectedPanel = new();

        public UsersView()
        {
            ScrollPanel = new VerticalScrollPanel
            {
                Content = new VerticalStackPanel
                {
                    Children = new IControl[]
                    {
                        new TextBlock { Text = "Connected users" },
                        connectedPanel,
                        new HorizontalSeparator(),
                        new TextBlock { Text = "Detected users" },
                        detectedPanel
                    }
                },
                ScrollBarBackground = new Character(),
                ScrollBarForeground = new Character()
            };

            Content = ScrollPanel;

            Program.Network.NodesDetection.DetectedNodes.CollectionChanged += RefreshDetectedUsers;
        }

        public VerticalScrollPanel ScrollPanel { get; }

        public void RefreshConnectedUsers()
        {
            Window.UiAction(() =>
            {
                connectedPanel.Children = new List<IControl>();
                Program.Network.Nodes.Where(x => x.Ready).ForEach(x =>
                {
                    connectedPanel.Add(new VerticalStackPanel
                    {
                        Children = new[]
                        {
                            new TextBlock(),
                            new TextBlock
                            {
                                Text = $"{x.User.Nickname} / {x.Host.Endpoint.Address}"
                            },
                            new TextBlock
                            {
                                Text = RsaFingerprint.GetMd5(x.NodeRsa.Rsa.ExportRSAPublicKey())
                            }
                        }
                    });
                });
            });
        }

        private void RefreshDetectedUsers(object sender, NotifyCollectionChangedEventArgs e)
        {
            Window.UiAction(() =>
            {
                detectedPanel.Children = new List<IControl>();
                Program.Network.NodesDetection.DetectedNodes.ForEach(x =>
                {
                    detectedPanel.Add(new TextBlock
                    {
                        Text = $"{x.Nickname} / {x.IpAddress}"
                    });
                });
            });
        }
    }
}