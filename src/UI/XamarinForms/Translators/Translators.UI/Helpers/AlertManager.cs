using System;
using System.Linq;
using System.Threading.Tasks;
using Translators.Models.Interfaces;

namespace Translators.UI.Helpers
{
    public class AlertManager : IAlertManager
    {
        public async Task<T> Display<T>(string title, string cancel, params string[] items)
            where T : Enum
        {
            var generatedItems = items.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            var result = await NavigationManager.GetCurrentPage().DisplayActionSheet(title, cancel, null, generatedItems);
            var index = Array.IndexOf(items, result);
            if (index == -1)
                return (T)(object)0;
            index++;
            //index += Array.IndexOf(items, result) - Array.IndexOf(generatedItems, result);
            return (T)(object)index;
        }

        public async Task<bool> DisplayQuestion(string title, string question)
        {
            var result = await NavigationManager.GetCurrentPage().DisplayAlert(title, question, "بلی", "انصراف");
            return result;
        }

        public async Task<string> DisplayPromptAsync(string title, string question)
        {
            return await NavigationManager.GetCurrentPage().DisplayPromptAsync(title, question);
        }

        public async Task<string> Display(string title, string cancel, params string[] items)
        {
            return await NavigationManager.GetCurrentPage().DisplayActionSheet(title, cancel, null, items);
        }

        public async Task Alert(string title, string message)
        {
            await NavigationManager.GetCurrentPage().DisplayAlert(title, message, "باشه");
        }
    }
}
