using System;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface IAlertManager
    {
        Task<T> Display<T>(string title, string cancel, params string[] items)
            where T : Enum;
    }
}
