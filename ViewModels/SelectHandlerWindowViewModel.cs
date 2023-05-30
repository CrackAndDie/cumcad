using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using cumcad.Models.Other;
using cumcad.Models.Factories;
using cumcad.Models.Classes;

namespace cumcad.ViewModels
{
    internal class SelectHandlerWindowViewModel : BindableBase
    {
        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { SetProperty(ref selectedIndex, value); }
        }

        private bool? isSelected;

        public List<HandlerType> ListViewItems => HandlerFactory.HandlerItems;

        #region Commands
        public ICommand DoneCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        #endregion

        internal SelectHandlerWindowViewModel()
        {
            DoneCommand = new DelegateCommand(OnDoneCommand);
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);
        }

        async internal Task<int?> GetSelectionTask()
        {
            await Task.Run(() =>
            {
                while (isSelected == null)
                {

                }
            });
            return (bool)isSelected ? (int?)SelectedIndex : null;
        }

        private void OnDoneCommand(object paramenter)
        {
            if (SelectedIndex != -1)
            {
                isSelected = true;
                (paramenter as Window).Close();
            }
            else
            {
                MessageBoxFactory.Show("You should select a Handler, you're f*cking asshole", MessageBoxFactory.INFO_LOGO);
            }
        }

        private void OnCloseWindowCommand(object paramenter)
        {
            isSelected = false;
            (paramenter as Window).Close();
        }
    }
}
