using Lanchat.Common.NetworkLib;
using Lanchat.Xamarin.ViewModels;
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
        }
    }
}
