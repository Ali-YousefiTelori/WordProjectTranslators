using System.Collections.Generic;
using System.Text;
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

        public static void AppendLineBefore(this StringBuilder builder, string text)
        {
            if (builder.Length == 0)
                builder.Append(text);
            else
            {
                builder.Append("\r\n");
                builder.Append(text);
            }
        }
    }
}
