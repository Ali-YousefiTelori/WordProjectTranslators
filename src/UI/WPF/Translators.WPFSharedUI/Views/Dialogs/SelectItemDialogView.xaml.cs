// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Translators.UI.Views.Dialogs
{
    public sealed partial class SelectItemDialogView : DialogControl
    {
        public SelectItemDialogView(string title, string cancelText, string[] items)
        {
            this.InitializeComponent();
            TitleText.Text = title;
            OkButton.Content = cancelText;
            OkButton.Click += OkButton_Click;
            ListItems.ItemsSource = items;
            ListItems.SelectionChanged += ListItems_SelectionChanged;
            GetNoText = () => OkButton.Content.ToString();
        }

        private void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnYesClick?.Invoke(ListItems.SelectedItem.ToString(), ListItems.SelectedItem.ToString());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            OnNoClick?.Invoke(GetNoText());
        }
    }
}
