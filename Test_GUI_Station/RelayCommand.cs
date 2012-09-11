using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace Test_GUI_Station
{
   /// <summary>
   /// This class was copied from a Solution found at
   /// http://reedcopsey.com/talks/implementing-parallel-patterns-in-net/
   /// </summary>
   class RelayCommand : ICommand
   {
      #region private fields
      private readonly Action execute;
      private readonly Func<bool> canExecute;
      #endregion

      /// <summary>
      /// Initializes a new instance of the RelayCommand class
      /// </summary>
      /// <param name="execute">The execute delegate.</param>
      public RelayCommand(Action execute)
         : this(execute, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the RelayCommand class
      /// </summary>
      /// <param name="execute">The execute delegate.</param>
      /// <param name="canExecute">The CanExecute delegate.</param>
      public RelayCommand(Action execute, Func<bool> canExecute)
      {
         this.execute = execute;
         this.canExecute = canExecute;
      }

      public void Execute(object parameter)
      {
         this.execute();
      }

      public bool CanExecute(object parameter)
      {
         bool result = this.canExecute == null ? true : this.canExecute();
         return result;
      }

      public event EventHandler CanExecuteChanged
      {
         add
         {
            CommandManager.RequerySuggested += value;
         }
         remove
         {
            CommandManager.RequerySuggested -= value;
         }
      }
   }
}
