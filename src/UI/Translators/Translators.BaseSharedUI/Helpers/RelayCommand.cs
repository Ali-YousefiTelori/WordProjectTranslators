using Translators.Models.Interfaces;

namespace Translators.UI.Helpers
{
#if (WPF)
    public class RelayCommand<T> : Command<T>, ICommand<T>
#else
    public class RelayCommand<T> : Command, ICommand<T>
#endif
    {
        public RelayCommand(Action<T> execute) : base((o) => execute((T)o))
        {

        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute) : base((o) => execute((T)o), (o) => canExecute((T)o))
        {

        }

        public Func<T, Task> AfterRun { get; set; }
    }

    public class RelayCommand : Command, ICommand
    {
        public RelayCommand(Action execute) : base(() => execute())
        {

        }

        public RelayCommand(Action execute, Func<bool> canExecute) : base(() => execute(), () => canExecute())
        {

        }

        public Func<Task> AfterRun { get; set; }
    }
}
