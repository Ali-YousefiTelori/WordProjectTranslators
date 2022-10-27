// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Translators.UI.Views.Components
{
    public sealed partial class VerseComponentView : UserControl
    {
        public VerseComponentView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty SourceViewModelProperty = DependencyProperty.Register("SourceViewModel", typeof(object), typeof(VerseComponentView), new PropertyMetadata());

        public object SourceViewModel
        {
            get { return (object)GetValue(SourceViewModelProperty); }
            set { SetValue(SourceViewModelProperty, value); }
        }
    }
}
