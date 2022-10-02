namespace Translators.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public TabBar TabBar
        {
            get
            {
                return MainTabBar;
            }
        }
    }
}
