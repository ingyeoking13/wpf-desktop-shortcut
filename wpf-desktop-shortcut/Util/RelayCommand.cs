using System;
using System.Windows.Input;

namespace wpf_desktop_shortcut.Util
{
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        Action<T> _action;
        Predicate<object> _canExecue;

        public RelayCommand(Action<T> action, Predicate<object> pred=null)
        {
            _action = action;
            _canExecue = pred;
        }
        public bool CanExecute(object parameter)
        {
            if (_canExecue == null) 
                return true;
            return _canExecue.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _action?.Invoke((T)parameter);
        }
    }
}
