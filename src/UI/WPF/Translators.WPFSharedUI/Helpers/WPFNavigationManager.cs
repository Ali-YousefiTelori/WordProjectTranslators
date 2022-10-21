using System.Collections;

namespace Translators.UI.Helpers
{
    public class WPFNavigationManager : INavigation
    {
        Frame MainFrame { get; set; }
        public WPFNavigationManager(Frame frame)
        {
            MainFrame = frame;
            NavigationStack = this;
        }

        public INavigation NavigationStack { get; set; }

        public IEnumerator<Page> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public async Task PopAsync()
        {
            MainFrame.GoBack();
        }

        public async Task PushAsync(Page page)
        {
            MainFrame.Navigate(page);
        }

        public Task RemovePage(Page page)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
