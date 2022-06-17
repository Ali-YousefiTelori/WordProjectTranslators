using System.Collections.Generic;
using Translators.Contracts.Common;
using Translators.Converters;

namespace System
{
    public static class Extensions
    {
        public static string GetPersianValue(this List<ValueContract> values)
        {
            return LanguageValueBaseConverter.GetValue(values, false, "fa-ir");
        }

        public static string GetMainValue(this List<ValueContract> values)
        {
            return LanguageValueBaseConverter.GetValue(values, true, null);
        }
    }
}
