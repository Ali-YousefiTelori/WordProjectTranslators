using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Translators.Contracts.Common;

namespace Translators.Converters
{
    public class LanguageValueBaseConverter
    {
        public bool IsMain { get; set; }
        public string LanguageCode { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<ValueContract> values)
            {
                return GetValue(values, IsMain, LanguageCode);
            }
            return $"Value {value?.GetType().Name} not found!";
        }

        public static string GetValue(List<ValueContract> values, bool isMain, string languageCode, bool isTransliteration = false)
        {
            if (values != null)
            {
                ValueContract find;
                if (isTransliteration)
                    find = values.FirstOrDefault(x => x.IsTransliteration);
                else
                    find = values.FirstOrDefault(x => !x.IsTransliteration && (x.IsMain == isMain || x.Language?.Code == languageCode));
                if (find == null)
                    return values.FirstOrDefault()?.Value;
                return find.Value;
            }
            return $"Value not found!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
