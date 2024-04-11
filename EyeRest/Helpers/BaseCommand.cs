using EyeRest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace EyeRest.Helpers
{
    public class BaseCommand : ICommand
    {
        #region Fields
        private readonly Action execute;
        private readonly Predicate<object>? canExecute;
        #endregion
        #region Constructor
        public BaseCommand(Action execute, Predicate<object>? canExecute =null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }
        #endregion
        #region ICommand Members
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (canExecute == null)
                return true;
            else
                return true;
        }

        public void Execute(object? parameter)
        {
            execute();
        }

        #endregion

    }
}
