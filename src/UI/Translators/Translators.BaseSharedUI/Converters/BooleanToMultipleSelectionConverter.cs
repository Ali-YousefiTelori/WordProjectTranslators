using System.Globalization;

namespace Translators.UI.Converters
{
    public class BooleanToMultipleSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool data && data)
            {
                return SelectionMode.Multiple;
            }

            return default(SelectionMode);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
