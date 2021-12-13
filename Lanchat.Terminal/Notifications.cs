using System;
using System.Timers;
using Lanchat.Core.Identity;

namespace Lanchat.Terminal
{
    public class Notifications
    {
        private bool inactive;
        private readonly Timer timer = new(10000);

        public Notifications()
        {
            timer.Elapsed += (_, _) => inactive = true;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void ResetInactivityTimer()
        {
            timer.Enabled = true;
            inactive = false;
            Console.Title = "Lanchat";
        }

        public void ShowNotification()
        {
            if (inactive && Program.Config.UserStatus != UserStatus.DoNotDisturb)
            {
                Console.Title = "Lanchat [New messages]";
            }
        }
    }
}