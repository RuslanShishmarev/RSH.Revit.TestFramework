using System;
using System.Windows.Input;

namespace RSH.Revit.TestFramework.Models
{
    internal class RelayCommand : ICommand
    {
        private Action<object> _executeWithObj;
        private Action _execute;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._executeWithObj = execute;
            this._canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<object, bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _executeWithObj?.Invoke(parameter);
            _execute?.Invoke();
        }
    }
}