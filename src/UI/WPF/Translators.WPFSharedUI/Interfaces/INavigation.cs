namespace Translators.UI.Interfaces
{
    public interface INavigation : IEnumerable<Page>
    {
        INavigation NavigationStack { get; set; }
        Task PushAsync(Page page);
        Task RemovePage(Page page);
        Task PopAsync();
    }
}
