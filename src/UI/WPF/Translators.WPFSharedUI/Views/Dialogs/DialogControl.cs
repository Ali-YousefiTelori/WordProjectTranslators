namespace Translators.UI.Views.Dialogs
{
    public class DialogControl : UserControl
    {
        public Action<string, string> OnYesClick { get; set; }
        public Action<string> OnNoClick { get; set; }
        public Func<string> GetNoText { get; set; }
    }
}
