using Lanchat.Core;
using Lanchat.Xamarin.ViewModels;
using System.Linq;
using Xamarin.Forms;

namespace Lanchat.Xamarin.Pages
{
    public partial class ChatPage
    {
        public ChatPage(P2P network)
        {
            InitializeComponent();
            BindingContext = new ChatViewModel(network);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Input.Focus();

            MessagingCenter.Subscribe<object>(this, "LogUpdated", (sender) => {
                Log.ScrollTo(Log.ItemsSource.OfType<object>().Last(), ScrollToPosition.MakeVisible, true);
            });
        }
    }
}
