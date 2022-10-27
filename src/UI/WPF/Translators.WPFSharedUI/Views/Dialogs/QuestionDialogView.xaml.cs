// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Translators.UI.Views.Dialogs
{
    public sealed partial class QuestionDialogView : DialogControl
    {
        public QuestionDialogView(string title, string OkText, string cancelText = "")
        {
            this.InitializeComponent();
            if (string.IsNullOrEmpty(cancelText))
                NoButton.Visibility = Visibility.Collapsed;
            TitleText.Text = title;
            NoButton.Content = cancelText;
            YesButton.Content = OkText;
            NoButton.Click += NoButton_Click;
            YesButton.Click += YesButton_Click;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            OnYesClick?.Invoke(YesButton.Content.ToString(), null);
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            OnNoClick?.Invoke(NoButton.Content.ToString());
        }
    }
}
