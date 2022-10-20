namespace Translators.UI.Helpers
{
    public static class AlertExtensions
    {
        public static Task DisplayAlert(this Page page, string title, string message, string cancel)
        {
            throw new NotImplementedException();
        }

        public static Task<bool> DisplayAlert(this Page page, string title, string message, string accept, string cancel)
        {
            return Task.FromResult(true);
        }

        public static Task DisplayAlert(this Page page, string title, string message, string cancel, FlowDirection flowDirection)
        {
            throw new NotImplementedException();
        }

        public static Task<bool> DisplayAlert(this Page page, string title, string message, string accept, string cancel, FlowDirection flowDirection)
        {
            return Task.FromResult(true);
        }

        public static Task<string> DisplayPromptAsync(this Page page, string title, string message)
        {
            return Task.FromResult("");
        }

        public static Task<string> DisplayActionSheet(this Page page, string title, string cancel, string destruction, params string[] buttons)
        {
            return Task.FromResult("");
        }
    }
}
