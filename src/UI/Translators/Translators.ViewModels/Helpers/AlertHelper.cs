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

        public static Task<string> Display(string title, string cancel, params string[] items)
        {
            return Current.Display(title, cancel, items);
        }

        public static Task<string> DisplayPrompt(string title, string question)
        {
            return Current.DisplayPromptAsync(title, question);
        }

        public static async Task Alert(string title, string message)
        {
           await Current.Alert(title, message);
        }

        public static Task<bool> DisplayQuestion(string title, string question)
        {
            return Current.DisplayQuestion(title, question);
        }
    }
}
