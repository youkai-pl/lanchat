using System;
using System.Diagnostics;
using ConsoleGUI.Controls;
using Lanchat.Core.FileTransfer;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface
{
    public class FileTransferMonitor : TextBlock
    {
        private long totalProgress;
        private long parts;
        
        public FileTransferMonitor()
        {
            Color = ConsoleColor.Gray;
            Text = string.Format(Resources._NoFileReceiveRequest);
        }

        public void OnFileTransferRequestAccepted(object sender, FileTransferRequest e)
        {
            parts += e.Parts;
            e.PropertyChanged += (_, _) =>
            {
                totalProgress++;
                Text = $"{totalProgress}/{parts}";
            };
        }

        public void OnFileReceiveFinished(object sender, FileTransferRequest e)
        {
            parts -= e.Parts;
            totalProgress -= e.Parts;
            Text = $"{totalProgress}/{parts}";
        }

        public void OnFileTransferError(object sender, FileTransferException e)
        {
            totalProgress -= e.Request.PartsTransferred;
            parts -= e.Request.Parts;
        }
    }
}