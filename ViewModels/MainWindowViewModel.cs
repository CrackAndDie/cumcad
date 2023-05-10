using cumcad.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using cumcad.ViewModels.Base;
using cumcad.Models.Helpers;
using System.Collections.ObjectModel;
using cumcad.Models;
using Prism.Mvvm;
using System.Threading;
using cumcad.Models.Factories;
using cumcad.Models.Other.MyEventArgs;
using cumcad.Models.Classes;

namespace cumcad.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        public MainWindowModel mainWindowModel = new MainWindowModel();

        private WindowState currentWindowState;
        public WindowState CurrentWindowState
        {
            get { return currentWindowState; }
            set { SetProperty(ref currentWindowState, value); OnStateChanged(currentWindowState); }
        }

        public int SelectedTabIndex
        {
            get { return mainWindowModel.SelectedTabIndex; }
            set { mainWindowModel.SelectedTabIndex = value; }
        }

        public Visibility MaximizeButtonVisibility => mainWindowModel.MaximizeButtonVisibility;

        public Visibility RestoreButtonVisibility => mainWindowModel.RestoreButtonVisibility;

        public Visibility ProgressBarVisibility => mainWindowModel.ProgressBarVisibility;

        public Visibility CheckAllDoneVisibility => mainWindowModel.CheckAllDoneVisibility;

        public Page MainFrameSource => mainWindowModel.MainFrameSource;

        public ReadOnlyObservableCollection<TabItemClass> TabItems => mainWindowModel.MainTabsModel.TabItems;

        #region Commands
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand RestoreWindowCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }

        public ICommand OpenFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }

        public ICommand AddEditorCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            mainWindowModel.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            MinimizeWindowCommand = new DelegateCommand(OnMinimizeWindowCommand);
            MaximizeWindowCommand = new DelegateCommand(OnMaximizeWindowCommand);
            RestoreWindowCommand = new DelegateCommand(OnRestoreWindowCommand);
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);

            OpenFileCommand = new DelegateCommand(OnOpenFileCommand);
            SaveFileCommand = new DelegateCommand(OnSaveFileCommand);

            AddEditorCommand = new DelegateCommand(OnAddEditorCommand);
            
            WaiterHelper.AddWaiter();

            OnStateChanged(WindowState.Normal);
            mainWindowModel.SelectedTabIndex = 0;

            WaiterHelper.RemoveWaiter();
        }

        private void OnAddEditorCommand(object parameter)
        {
            mainWindowModel.CreateEditor();
        }

        private void OnStateChanged(WindowState state)
        {
            mainWindowModel.ChangeWindowIconState(state == WindowState.Maximized);
        }

        private void OnSaveFileCommand(object parameter)
        {

        }

        private void OnOpenFileCommand(object parameter)
        {

        }

        private void OnMinimizeWindowCommand(object paramenter)
        {
            (paramenter as Window).WindowState = WindowState.Minimized;
        }

        private void OnMaximizeWindowCommand(object paramenter)
        {
            (paramenter as Window).WindowState = WindowState.Maximized;
        }

        private void OnRestoreWindowCommand(object paramenter)
        {
            (paramenter as Window).WindowState = WindowState.Normal;
        }

        private void OnCloseWindowCommand(object paramenter)
        {
            (paramenter as Window).Close();
        }
    }
}
