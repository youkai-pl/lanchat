using System;
using System.IO;
using System.Security;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.FileTransfer
{
    public class Send : ICommand
    {
        public string Alias => "send";
        public int ArgsCount => 2;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node == null)
            {
                Window.Writer.WriteError(Resources._UserNotFound);
                return;
            }

            try
            {
                node.FileSender.CreateSendRequest(args[1]);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case FileNotFoundException:
                    case UnauthorizedAccessException:
                    case SecurityException:
                    case PathTooLongException:
                    case ArgumentException:
                        Window.Writer.WriteError(string.Format(Resources._CannotAccessFile, args[1]));
                        break;

                    case InvalidOperationException:
                        Window.Writer.WriteError(Resources._FileTransferInProgress);
                        break;
                }
            }
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}