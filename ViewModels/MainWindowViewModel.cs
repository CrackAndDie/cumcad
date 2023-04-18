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

namespace cumcad.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        internal MainTabsModel mainTabsModel = new MainTabsModel();

        private WindowState currentWindowState;
        public WindowState CurrentWindowState
        {
            get { return currentWindowState; }
            set { SetProperty(ref currentWindowState, value); OnStateChanged(currentWindowState); }
        }

        private Visibility maximizeButtonVisibility;
        public Visibility MaximizeButtonVisibility
        {
            get { return maximizeButtonVisibility; }
            set { SetProperty(ref maximizeButtonVisibility, value); }
        }

        private Visibility restoreButtonVisibility;
        public Visibility RestoreButtonVisibility
        {
            get { return restoreButtonVisibility; }
            set { SetProperty(ref restoreButtonVisibility, value); }
        }

        private Visibility progressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get { return progressBarVisibility; }
            set { SetProperty(ref progressBarVisibility, value); }
        }

        private Visibility checkAllDoneVisibility;
        public Visibility CheckAllDoneVisibility
        {
            get { return checkAllDoneVisibility; }
            set { SetProperty(ref checkAllDoneVisibility, value); }
        }

        private Page mainFrameSource;
        public Page MainFrameSource
        {
            get { return mainFrameSource; }
            set { SetProperty(ref mainFrameSource, value); }
        }

        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set { SetProperty(ref selectedTabIndex, value); TabSelectionChanged(selectedTabIndex); }
        }

        public ReadOnlyObservableCollection<TabItemClass> TabItems => mainTabsModel.TabItems;

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
            mainTabsModel.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            MinimizeWindowCommand = new DelegateCommand(OnMinimizeWindowCommand);
            MaximizeWindowCommand = new DelegateCommand(OnMaximizeWindowCommand);
            RestoreWindowCommand = new DelegateCommand(OnRestoreWindowCommand);
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);

            OpenFileCommand = new DelegateCommand(OnOpenFileCommand);
            SaveFileCommand = new DelegateCommand(OnSaveFileCommand);

            AddEditorCommand = new DelegateCommand(OnAddEditorCommand);

            WaiterHelper.CollectionChanged += OnStaticAllDoneChanged;
            WaiterHelper.AddWaiter();

            // OnActionChanged(false);
            OnStateChanged(WindowState.Normal);
            SelectedTabIndex = 0;

            WaiterHelper.RemoveWaiter();
        }

        private void OnStaticAllDoneChanged(object sender, EventArgs e)
        {
            OnActionChanged(WaiterHelper.GetWaiterStatus());
        }

        private void GoToPage(Page page)
        {
            if (MainFrameSource != page)
            {
                MainFrameSource = page;
            }
        }

        private void TabSelectionChanged(int index)
        {
            if (index == 0)
            {
                GoToPage(null);
            }
            else
            {
                // I don't want to fix that
                try
                {
                    GoToPage(EditorsHelper.GetPageView(index - 1));
                }
                catch (ArgumentOutOfRangeException)
                {
                    GoToPage(null);
                }
            }
        }

        async private void OnAddEditorCommand(object parameter)
        {
            var result = await SelectEditorFactory.OpenSelectEditorWindow();
            if ((bool)result.IsSelected)
            {
                mainTabsModel.AddNewItem(result.IconColor).OnRemove += OnItemRemove;
                GoToPage(EditorsHelper.AddNewEditorPage(result));
                SelectedTabIndex = EditorsHelper.GetListCount();
            }
        }

        private void OnItemRemove(object sender, EventArgs args)
        {
            var item = sender as TabItemClass;
            item.OnRemove -= OnItemRemove;
            int ind = mainTabsModel.IndexOf(item);
            mainTabsModel.RemoveItem(ind);
            EditorsHelper.RemoveAt(ind - 1);
        }

        private void OnActionChanged(bool done)
        {
            if (done)
            {
                ProgressBarVisibility = Visibility.Collapsed;
                CheckAllDoneVisibility = Visibility.Visible;
            }
            else
            {
                ProgressBarVisibility = Visibility.Visible;
                CheckAllDoneVisibility = Visibility.Collapsed;
            }
        }

        private void OnStateChanged(WindowState state)
        {
            if (state == WindowState.Maximized)
            {
                MaximizeButtonVisibility = Visibility.Collapsed;
                RestoreButtonVisibility = Visibility.Visible;
            }
            else
            {
                MaximizeButtonVisibility = Visibility.Visible;
                RestoreButtonVisibility = Visibility.Collapsed;
            }
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
