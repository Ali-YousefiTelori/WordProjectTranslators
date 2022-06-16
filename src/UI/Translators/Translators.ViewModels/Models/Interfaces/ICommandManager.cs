using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface ICommandManager
    {
        ICommand<T> Create<T>(Func<T, Task> func);
        ICommand Create(Func<Task> func);
    }
}
