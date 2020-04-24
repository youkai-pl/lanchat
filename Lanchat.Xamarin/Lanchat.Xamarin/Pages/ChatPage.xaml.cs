using Lanchat.Common.NetworkLib;
using Lanchat.Xamarin.ViewModels;
using System.Linq;
using Xamarin.Forms;

namespace Lanchat.Xamarin.Pages
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage(Network network)
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
