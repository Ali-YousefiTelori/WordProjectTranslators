using System.Globalization;
using Translators.Contracts.Common;

#if (CSHTML5)
using System.Linq;
using System;
using System.Collections.Generic;
#endif

namespace Translators.UI.Converters
{
    public class ParagraphConverter : BaseConverter
    {
        public bool IsTranslated { get; set; }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
