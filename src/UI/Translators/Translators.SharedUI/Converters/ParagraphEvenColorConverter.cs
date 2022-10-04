using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Translators.Models;
using Translators.SharedUI.Helpers;

namespace Translators.SharedUI.Converters
{
    public class ParagraphEvenColorConverter: EvenColorConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ParagraphBaseModel paragraph)
            {
                if (paragraph.IsSelected)
                {
                    return ApplicationSharedHelper.GetColorFromHex("#009404");
                }
                return base.Convert(paragraph.IsEven, targetType, parameter, culture);
            }
            return base.Convert(value, targetType, parameter, culture);
        }
    }
}
