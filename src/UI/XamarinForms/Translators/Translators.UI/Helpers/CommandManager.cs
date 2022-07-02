using System;
using System.Threading.Tasks;
using Translators.Models.Interfaces;
using Translators.ViewModels;

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
                    await BaseViewModel.AlertExcepption(ex);
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
                    await BaseViewModel.AlertExcepption(ex);
                }
            });
        }
    }
}
