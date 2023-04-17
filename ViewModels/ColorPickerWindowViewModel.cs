using cumcad.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace cumcad.ViewModels
{
    internal class ColorPickerWindowViewModel
    {
        #region Commands
        public ICommand MouseUpCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        #endregion

        internal ColorPickerWindowViewModel()
        {
            MouseUpCommand = new DelegateCommand(OnMouseUpCommand);
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);
        }

        private void OnMouseUpCommand(object parameter)
        {

        }

        private void OnCloseWindowCommand(object paramenter)
        {
            (paramenter as Window).Close();
        }
    }
}
