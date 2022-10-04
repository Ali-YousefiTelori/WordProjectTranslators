using System.Globalization;
using Translators.Contracts.Common;

namespace Translators.SharedUI.Converters
{
    public class ParagraphConverter : IValueConverter
    {
        public bool IsTranslated { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<WordContract> words)
            {
                if (IsTranslated)
                    return string.Join(" ", words.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => !x.IsMain && x.Language.Code == "fa-ir").Select(x => x.Value));
                else
                    return string.Join(" ", words.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => x.IsMain).Select(x => x.Value));
            }
            return "Type is not valid!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
