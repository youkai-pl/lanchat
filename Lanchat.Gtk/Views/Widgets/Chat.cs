using Gtk;
using Key = Gdk.Key;
using UI = Gtk.Builder.ObjectAttribute;
using WrapMode = Pango.WrapMode;

namespace Lanchat.Gtk.Views.Widgets
{
    public class Chat
    {
        [UI] private readonly ListBox chat;
        [UI] private readonly Entry input;
        [UI] private readonly ScrolledWindow scroll;

        private string lastMessageAuthor;

        public Chat(ScrolledWindow scroll, ListBox chat, Entry input)
        {
            this.scroll = scroll;
            this.chat = chat;
            this.input = input;

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