using System;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface ICommand<T>
    {
        Func<T, Task> AfterRun { get; set; }
    }

    public interface ICommand
    {
        Func<Task> AfterRun { get; set; }
    }
}
