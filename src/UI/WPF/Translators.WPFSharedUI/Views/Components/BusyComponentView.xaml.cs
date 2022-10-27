// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Translators.UI.Views.Components
{
    public sealed partial class BusyComponentView : UserControl
    {
        public BusyComponentView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning", typeof(bool), typeof(BusyComponentView), new PropertyMetadata(new PropertyChangedCallback(Changed)));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool value && value)
            {
                (d as BusyComponentView).Visibility = Visibility.Visible;
            }
            else
                (d as BusyComponentView).Visibility = Visibility.Collapsed;
        }

        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }
    }
}
