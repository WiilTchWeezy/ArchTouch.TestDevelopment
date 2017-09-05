using System.Windows.Input;
using Xamarin.Forms;

namespace ArcTouch.TestDevelopment.Views.Controls
{
    public class CustomListView : ListView
    {
        public ICommand NewPageCommand
        {
            get { return (ICommand)GetValue(NewPageCommandProperty); }
            set { SetValue(NewPageCommandProperty, value); }
        }

        public static readonly BindableProperty NewPageCommandProperty =
            BindableProperty.Create("NewPageCommand", typeof(ICommand), typeof(CustomListView), default(ICommand));

        public CustomListView()
        {
            ItemAppearing += CustomListView_ItemAppearing;
            ItemSelected += CustomListView_ItemSelected;
        }

        private void CustomListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
                SelectedItem = null;
        }

        private void CustomListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (ItemsSource != null && e.Item != null)
            {
                var list = ItemsSource as System.Collections.IList;
                var indexOf = list.IndexOf(e.Item);

                if (indexOf == list.Count - 1)
                    NewPageCommand?.Execute(null);
            }
        }
    }
}
