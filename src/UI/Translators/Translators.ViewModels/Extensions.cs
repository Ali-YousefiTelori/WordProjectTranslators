using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using TranslatorApp.GeneratedServices;
using Translators.Shared.Converters;

namespace System
{
    public static class Extensions
    {
        public static string GetPersianValue(this ICollection<ValueContract> values)
        {
            return LanguageValueBaseConverter.GetValue(JsonConvert.DeserializeObject<List<Translators.Contracts.Common.ValueContract>>(JsonConvert.SerializeObject(values)), false, "fa-ir");
        }

        public static string GetMainValue(this ICollection<ValueContract> values)
        {
            return LanguageValueBaseConverter.GetValue(JsonConvert.DeserializeObject<List<Translators.Contracts.Common.ValueContract>>(JsonConvert.SerializeObject(values)), true, null);
        }
    }
}