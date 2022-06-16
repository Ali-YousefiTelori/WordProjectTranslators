using System;
using System.Threading.Tasks;
using Translators.Models.Interfaces;

namespace Translators.UI.Helpers
{
    public class AlertManager : IAlertManager
    {
        public async Task<T> Display<T>(string title, string cancel, params string[] items)
            where T: Enum
        {
            var result = await NavigationManager.GetCurrentPage().DisplayActionSheet(title, cancel, null, items);
            var index = Array.IndexOf(items, result);
            if (index == -1)
                return (T)(object)0;
            index++;
            return (T)(object)index;
        }
    }
}
