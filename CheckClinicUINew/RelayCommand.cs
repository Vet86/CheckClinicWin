using System;
using System.Windows.Input;

namespace CheckClinicUI.Base
{
   public class RelayCommand : ICommand, IDisposable
   {
      private Action<object> _execute;
      private Func<object, bool> _canExecute;

      public event EventHandler CanExecuteChanged
      {
         add { CommandManager.RequerySuggested += value; }
         remove { CommandManager.RequerySuggested -= value; }
      }

      public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
      {
         _execute = execute;
         _canExecute = canExecute;
      }

      public bool CanExecute(object parameter)
      {
         return _canExecute == null || _canExecute(parameter);
      }

      public void Execute(object parameter)
      {
         _execute(parameter);
      }

      public void TryExecute(object parameter)
      {
         if (CanExecute(parameter))
            Execute(parameter);
      }

      public void UpdateCanExecute()
      {
         CommandManager.InvalidateRequerySuggested();
      }

      public void Dispose()
      {
         _execute = null;
         _canExecute = null;
      }
   }
}
