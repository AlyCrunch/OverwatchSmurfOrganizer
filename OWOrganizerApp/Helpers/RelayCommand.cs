using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OWOrganizerApp.Helpers
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => this.canExecute == null || this.canExecute(parameter);
        public void Execute(object parameter) => this.execute(parameter);
    }
    public class RelayCommandAsync : ICommand
    {
        private readonly Func<object, Task> executedMethod;
        private readonly Func<object, bool> canExecuteMethod;

        public event EventHandler CanExecuteChanged;
        public RelayCommandAsync(Func<object, Task> execute) : this(execute, null) { }
        public RelayCommandAsync(Func<object, Task> execute, Func<object, bool> canExecute)
        {
            this.executedMethod = execute ?? throw new ArgumentNullException("execute");
            this.canExecuteMethod = canExecute;
        }

        public bool CanExecute(object parameter) => this.canExecuteMethod == null || this.canExecuteMethod(parameter);
        public async void Execute(object parameter) => await this.executedMethod(parameter);
        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
