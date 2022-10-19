using Translators.Models.Interfaces;
using Translators.ViewModels;

namespace Translators.UI.Helpers
{
    public class CommandManager : ICommandManager
    {
        public ICommand<T> Create<T>(Func<T, Task> func)
        {
            RelayCommand<T> command = null;
            command = new RelayCommand<T>(async (data) =>
            {
                try
                {
                    await func(data);
                }
                catch (Exception ex)
                {
                    await BaseViewModel.AlertExcepption(ex);
                }
                finally
                {
                    if (command.AfterRun != null)
                        await AfterRun(data, command);
                }
            });
            return command;
        }

        public ICommand Create(Func<Task> func)
        {
            RelayCommand command = null;
            command = new RelayCommand(async () =>
            {
                try
                {
                    await func();
                }
                catch (Exception ex)
                {
                    await BaseViewModel.AlertExcepption(ex);
                }
                finally
                {
                    if (command.AfterRun != null)
                        await AfterRun(command);
                }
            });
            return command;
        }

        public async Task AfterRun(ICommand command)
        {
            try
            {
                await command.AfterRun();
            }
            catch (Exception ex)
            {
                await BaseViewModel.AlertExcepption(ex);
            }
        }

        public async Task AfterRun<T>(T data, ICommand<T> command)
        {
            try
            {
                await command.AfterRun(data);
            }
            catch (Exception ex)
            {
                await BaseViewModel.AlertExcepption(ex);
            }
        }
    }
}
