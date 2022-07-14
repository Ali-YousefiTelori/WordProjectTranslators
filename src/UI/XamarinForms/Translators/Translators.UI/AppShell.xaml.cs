using Xamarin.Forms;

namespace Translators.UI
{
    public partial class AppShell : Xamarin.Forms.Shell
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
