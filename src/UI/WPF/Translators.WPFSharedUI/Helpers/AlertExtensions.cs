using Translators.UI.Views.Dialogs;
using Translators.UI.WPFCore;

namespace Translators.UI.Helpers
{
    public static class AlertExtensions
    {
        public static async Task DisplayAlert(this Page page, string title, string message, string cancel)
        {
            GetWindow(title, new QuestionDialogView(message, cancel));
        }

        public static async Task<bool> DisplayAlert(this Page page, string title, string message, string accept, string cancel)
        {
            return GetWindow(title, new QuestionDialogView(message, accept, cancel)).Button == accept;
        }

        public static async Task DisplayAlert(this Page page, string title, string message, string cancel, FlowDirection flowDirection)
        {
            GetWindow(title, new QuestionDialogView(message, cancel));
        }

        public static async Task<bool> DisplayAlert(this Page page, string title, string message, string accept, string cancel, FlowDirection flowDirection)
        {
            return GetWindow(title, new QuestionDialogView(message, accept, cancel)).Button == accept;
        }

        public static async Task<string> DisplayPromptAsync(this Page page, string title, string message)
        {
            string accept = "بلی";
            var result = GetWindow(title, new InputDialogView(message, accept, "انصراف"));
            if (result.Button == accept)
            {
                return result.Input;
            }
            return "";
        }

        public static async Task<string> DisplayActionSheet(this Page page, string title, string cancel, string destruction, params string[] buttons)
        {
            var result = GetWindow(title, new SelectItemDialogView(title, cancel, buttons));
            return string.IsNullOrEmpty(result.Button) ? result.Input : result.Button;
        }

        public static (string Button, string Input) GetWindow(string title, DialogControl control)
        {
            var window = new Window()
            {
                Owner = App.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Title = title,
                Padding = new Thickness(10),
                SizeToContent = SizeToContent.WidthAndHeight,
                Content = control,
                WindowStyle = WindowStyle.ToolWindow
            };

            string result = "", input = "";
            control.OnYesClick = (text, _input) =>
            {
                result = text;
                input = _input;
                window.Close();
            };

            control.OnNoClick = (text) =>
            {
                result = text;
                input = result;
                window.Close();
            };

            window.Closing += (s, e) =>
            {
                if (string.IsNullOrEmpty(input))
                    input = control.GetNoText?.Invoke();
            };
            window.ShowDialog();
            return (result, input);
        }
    }
}
