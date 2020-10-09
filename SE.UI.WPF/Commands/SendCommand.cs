using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE.UI.WPF.Commands
{
    public class SendCommand : ICommand
    {
        public Action ExecuteFunc
        {
            get;
            set;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event System.EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteFunc();
        }
    }
   
}
