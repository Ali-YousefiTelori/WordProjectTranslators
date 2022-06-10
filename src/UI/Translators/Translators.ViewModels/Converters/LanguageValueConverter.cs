using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Translators.Contracts.Common;

namespace Translators.Converters
{
    public class LanguageValueConverter : IValueConverter
    {
        public bool IsMain { get; set; }
        public string LanguageCode { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<ValueContract> values)
            {
                return values.FirstOrDefault(x => x.IsMain == IsMain || x.Language?.Code == LanguageCode).Value;
            }
            return $"Value {value?.GetType().Name} not found!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
