using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView.Platforms.WPF;
using Xamarin.Forms.Platform.WPF;

namespace Translators.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();

            var assembliesToInclude = new List<Assembly>()
            {
                typeof(Xamarin.CommunityToolkit.Effects.TouchEffect).GetTypeInfo().Assembly,
                typeof(MainWindow).GetTypeInfo().Assembly
            };

            Xamarin.Forms.Forms.Init(assembliesToInclude);
            PancakeViewRenderer.Init();
            LoadApplication(new Translators.UI.App());
        }
    }
}
