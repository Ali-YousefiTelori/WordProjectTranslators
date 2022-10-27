﻿// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Translators.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainTabbedPage : Page
    {
        public MainTabbedPage()
        {
            this.InitializeComponent();
            NavigationManager.Initialize(new WPFNavigationManager(BookFrame));
            NavigationManager.InitializeSearchNavigation(new WPFNavigationManager(SearchFrame));
            NavigationManager.InitializeSettingNavigation(new WPFNavigationManager(SettingsFrame));
        }
    }
}