using Gtk;
using Key = Gdk.Key;
using WrapMode = Pango.WrapMode;

namespace Lanchat.Gtk.Views.Widgets
{
    public class ChatWidget
    {
        private readonly ListBox chat;
        private readonly Entry input;
        private readonly ScrolledWindow scroll;

        private string lastMessageAuthor;

        public ChatWidget(MainWindow mainWindow)
        {
            scroll = mainWindow.Scroll;
            chat = mainWindow.Chat;
            input = mainWindow.Input;

            input.KeyReleaseEvent += InputOnKeyReleaseEvent;
        }

        public void AddChatEntry(string nickname, string message)
        {
            Application.Invoke(delegate
            {
                var box = new Box(Orientation.Vertical, 0);

                var sender = new Label
                {
                    Valign = Align.Start,
                    Halign = Align.Start,
                    Markup = $"<b>{nickname}</b>"
                };

                var content = new Label(message)
                {
                    Valign = Align.Start,
                    Halign = Align.Start,
                    Wrap = true,
                    LineWrapMode = WrapMode.Char,
                    Selectable = true
                };

                if (lastMessageAuthor != nickname)
                {
                    box.Add(sender);
                    lastMessageAuthor = nickname;
                }

                box.Add(content);
                chat.Add(new ListBoxRow {Child = box});
                chat.ShowAll();
                scroll.Vadjustment.Value = scroll.Vadjustment.Upper;
            });
        }

        private void InputOnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
        {
            if (args.Event.Key != Key.Return) return;
            AddChatEntry($"{Program.Config.Nickname}#0000", input.Text);
            Program.Network.BroadcastMessage(input.Text);
            input.Text = string.Empty;
        }
    }
}