using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Translators.SharedUI.Helpers;

namespace Translators.SharedUI.Converters
{
    public class BooleanToMultipleSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool data && data)
            {
                return SelectionMode.Multiple;
            }
            return SelectionMode.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
