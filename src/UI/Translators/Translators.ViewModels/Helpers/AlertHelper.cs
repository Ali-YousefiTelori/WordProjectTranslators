using System;
using System.Threading.Tasks;
using Translators.Models.Interfaces;

namespace Translators.Helpers
{
    public class AlertHelper
    {
        public static IAlertManager Current { get; set; }
        public static Task<T> Display<T>(string title, string cancel, params string[] items)
            where T : Enum
        {
            return Current.Display<T>(title, cancel, items);
        }
    }
}
