using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Translators.Models.Interfaces;

namespace Translators.UI.Helpers
{
    public class CommandManager : ICommandManager
    {
        public ICommand<T> Create<T>(Func<T, Task> func)
        {
            return new RelayCommand<T>(async (data) =>
            {
                try
                {
                    await func(data);
                }
                catch (Exception ex)
                {

                }
            });
        }

        public ICommand Create(Func<Task> func)
        {
            return new RelayCommand(async () =>
            {
                try
                {
                    await func();
                }
                catch (Exception ex)
                {

                }
            });
        }
    }
}
