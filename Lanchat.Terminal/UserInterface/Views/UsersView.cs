using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Terminal.Properties;

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
                        connectedPanel,
                        detectedPanel
                    }
                },
                ScrollBarBackground = new Character(),
                ScrollBarForeground = new Character(),
                ScrollUpKey = ConsoleKey.PageUp,
                ScrollDownKey = ConsoleKey.PageDown
            };

            Content = new DockPanel
            {
                FillingControl = ScrollPanel,
                DockedControl = new Border
                {
                    BorderPlacement = BorderPlacement.Top,
                    BorderStyle = BorderStyle.Single,
                    Content = new VerticalStackPanel
                    {
                        Children = new[]
                        {
                            new TextBlock
                            {
                                Text = Resources.YourRsa
                            },
                            new TextBlock
                            {
                                Text = RsaFingerprint.GetMd5(Program.Network.LocalRsa.Rsa.ExportRSAPublicKey())
                            }
                        }
                    }
                },
                Placement = DockPanel.DockedControlPlacement.Bottom
            };

            Program.Network.NodesDetection.DetectedNodes.CollectionChanged += RefreshDetectedUsers;
        }

        public VerticalScrollPanel ScrollPanel { get; }

        public void RefreshUsersView()
        {
            Window.UiAction(() =>
            {
                connectedPanel.Children = new List<IControl>();
                Program.Network.Nodes.Where(x => x.Ready).ForEach(x =>
                {
                    connectedPanel.Add(new VerticalStackPanel
                    {
                        Children = new IControl[]
                        {
                            new TextBlock
                            {
                                Text = $"{x.User.Nickname} - {x.Host.Endpoint.Address}"
                            },
                            new TextBlock
                            {
                                Text = RsaFingerprint.GetMd5(x.NodeRsa.Rsa.ExportRSAPublicKey())
                            },
                            new HorizontalSeparator()
                        }
                    });
                });

                detectedPanel.Children = new List<IControl>();
                var connected = Program.Network.Nodes.Select(x => x.Host.Endpoint.Address);
                Program.Network.NodesDetection.DetectedNodes.Where(x => !connected.Any(y => y.Equals(x.IpAddress)))
                    .ForEach(x =>
                    {
                        detectedPanel.Add(new VerticalStackPanel
                        {
                            Children = new IControl[]
                            {
                                new TextBlock
                                {
                                    Text = $"{x.Nickname} - {x.IpAddress}"
                                },
                                new HorizontalSeparator()
                            }
                        });
                    });
            });
        }

        private void RefreshDetectedUsers(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshUsersView();
        }
    }
}