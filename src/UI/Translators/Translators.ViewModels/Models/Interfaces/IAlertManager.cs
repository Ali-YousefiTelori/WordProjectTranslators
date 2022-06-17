using System;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface IAlertManager
    {
        Task<T> Display<T>(string title, string cancel, params string[] items)
            where T : Enum;
        Task<string> Display(string title, string cancel, params string[] items);
        Task<string> DisplayPromptAsync(string title, string question);
        Task<bool> DisplayQuestion(string title, string question);
        Task Alert(string title, string message);
    }
}
