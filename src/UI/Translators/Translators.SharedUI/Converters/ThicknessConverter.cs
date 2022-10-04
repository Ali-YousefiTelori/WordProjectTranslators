using System.Globalization;

namespace Translators.SharedUI.Converters
{
    public class ThicknessConverter : IValueConverter
    {
        public bool IsLeft { get; set; } = true;
        public bool IsRight { get; set; } = true;
        public bool IsTop { get; set; } = true;
        public bool IsBottom { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int number)
            {
                int left = IsLeft ? number : 0;
                int right = IsRight ? number : 0;
                int top = IsTop ? number : 0;
                int bottom = IsBottom ? number : 0;
                return new Thickness(left, top, right, bottom);
            }

            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
