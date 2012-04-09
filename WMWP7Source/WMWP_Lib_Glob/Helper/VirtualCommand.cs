using System;
using System.Windows.Input;
using System.Diagnostics;

namespace Sozialhelden.Wheelmap.Lib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class VirtualCommand : ICommand
    {
        #region Internals

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;


        /// <summary>
        /// Create a new command which can't be disabled 
        /// </summary>
        /// <param name="execute">The execute.</param>
        public VirtualCommand(Action<object> execute)
            : this(execute, null)
        {
        }


        /// <summary>
        /// create a new command which is disableble
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute"> can execute.</param>
        public VirtualCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members
#if SILVERLIGHT
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void RaiseExecuteChanged()
        {
            try
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        public event EventHandler CanExecuteChanged;


        public void Execute(object parameter)
        {
            _execute(parameter);
        }

#else
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
#endif

        #endregion // ICommand Members

    }
}