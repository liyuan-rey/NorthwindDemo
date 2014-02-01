// RelayCommand.cs

namespace Northwind.WpfClient.Common
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Northwind.Common;

    public class RelayCommand : ICommand, IActiveAware
    {
        private readonly Action<object> _execute;

        public RelayCommand(string text, Action execute)
            : this(text, param => execute(), null)
        {
        }

        public RelayCommand(string text, Action<object> execute)
            : this(text, execute, null)
        {
        }

        public RelayCommand(string text, Action<object> execute, Predicate<object> canExecute)
        {
            ContractUtil.RequiresNotNull(execute, "execute");

            Text = text;
            _execute = execute;
            _canExecute = canExecute;
        }

        public string Text { get; set; }

        #region ICommand

        private readonly Predicate<object> _canExecute;

        private EventHandler _canExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            if (_isActive)
            {
                return false;
            }

            if (_canExecute == null)
            {
                return true;
            }

            try
            {
                return _canExecute(parameter);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                Dispatcher.CurrentDispatcher.CheckAccess();

                _canExecuteChanged = (EventHandler) Delegate.Combine(_canExecuteChanged, value);
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                Dispatcher.CurrentDispatcher.CheckAccess();

                _canExecuteChanged = (EventHandler) Delegate.Remove(_canExecuteChanged, value);
                CommandManager.RequerySuggested -= value;
            }
        }


        public void Execute(object parameter)
        {
            try
            {
                IsActive = true;
                _execute(parameter);
            }
            finally
            {
                IsActive = false;
            }
        }

        #endregion

        #region IActiveAware

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }

            private set
            {
                if (_isActive == value)
                    return;

                _isActive = value;

                if (IsActiveChanged != null)
                {
                    IsActiveChanged(this, new EventArgs());
                }
            }
        }

        public event EventHandler IsActiveChanged;

        #endregion
    }
}