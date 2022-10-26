using System;

#if (CSHTML5)
using System.Globalization;
using Windows.UI.Xaml.Controls;
#endif

namespace Translators.UI.Converters
{
    public class BooleanToMultipleSelectionConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool data && data)
            {
                return SelectionMode.Multiple;
            }

            return default(SelectionMode);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
