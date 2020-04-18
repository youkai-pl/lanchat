using System.Linq;
using Xamarin.Forms;

namespace Lanchat.Xamarin.Behaviors
{
    public class ListBehavior : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            bindable.ItemAppearing += (sender, e) =>
            {
                var lastItem = bindable.ItemsSource.OfType<object>().Last();
                bindable.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
            };
            base.OnAttachedTo(bindable);
        }
    }
}