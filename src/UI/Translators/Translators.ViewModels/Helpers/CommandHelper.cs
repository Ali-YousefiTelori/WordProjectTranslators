using System;
using System.Threading.Tasks;
using Translators.Models.Interfaces;

namespace Translators.Helpers
{
    public static class CommandHelper
    {
        public static ICommandManager Current { get; set; }

        public static ICommand<T> Create<T>(Func<T, Task> func)
        {
            return Current.Create<T>(func);
        }

        public static ICommand Create(Func<Task> func)
        {
            return Current.Create(func);
        }
    }
}
