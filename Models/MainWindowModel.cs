﻿using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.Models.Other.MyEventArgs;
using cumcad.ViewModels;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace cumcad.Models
{
    internal class MainWindowModel : BindableBase
    {
        public MainTabsModel MainTabsModel = new MainTabsModel();

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
            set { SetProperty(ref selectedTabIndex, value); TabSelectionChanged(value); }
        }

        public MainWindowModel() 
        {
            MainTabsModel.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            WaiterHelper.CollectionChanged += OnStaticAllDoneChanged;
        }

        #region Editors
        async internal void CreateEditor()
        {
            var result = await SelectorsFactory.OpenSelectEditorWindow();
            if ((bool)result.IsSelected)
            {
                AddEditor(result);
            }
        }

        async internal void CreateEditorFromAnother(object sender, EditorItemEventArgs args)
        {
            var editorModel = sender as EditorPageModel;
            var item = args.Parameter;
            AddEditor(new SelectEditorResult()
            {
                IsSelected = true,
                IconColor = await SelectorsFactory.OpenColorPickerWindow(),
                SelectedType = EditorType.FromEditor,
                ParentEditorItem = item,
                ParentEditorModel = editorModel
            });
        }

        private void AddEditor(SelectEditorResult result)
        {
            MainTabsModel.AddNewItem(result.IconColor).OnRemove += OnItemRemove;
            var editor = EditorsHandler.AddNewEditorPage(result);
            (editor.DataContext as EditorPageViewModel).RemoveFromInside += OnItemRemoveFromInside;
            (editor.DataContext as EditorPageViewModel).CreateFromEditorItem += CreateEditorFromAnother;
            GoToPage(editor);
            SelectedTabIndex = EditorsHandler.GetListCount();
        }

        private void OnItemRemove(object sender, EventArgs args)
        {
            var item = sender as TabItemClass;
            item.OnRemove -= OnItemRemove;
            int ind = MainTabsModel.IndexOf(item);
            RemoveEditor(ind - 1);
        }

        private void OnItemRemoveFromInside(object sender, EventArgs args)
        {
            int ind = EditorsHandler.IndexOf(sender as EditorPageViewModel);
            var tabItem = MainTabsModel.TabItems[ind + 1];
            tabItem.OnRemove -= OnItemRemove;
            RemoveEditor(ind);
        }

        private void RemoveEditor(int ind)
        {
            MainTabsModel.RemoveItem(ind + 1);
            (EditorsHandler.GetPageView(ind).DataContext as EditorPageViewModel).RemoveFromInside -= OnItemRemoveFromInside;
            (EditorsHandler.GetPageView(ind).DataContext as EditorPageViewModel).CreateFromEditorItem -= CreateEditorFromAnother;
            EditorsHandler.RemoveAt(ind);
        }
        #endregion

        private void TabSelectionChanged(int index)
        {
            GoToPage(index);
        }

        internal void GoToPage(Page page)
        {
            if (MainFrameSource != page)
            {
                MainFrameSource = page;
            }
        }

        internal void GoToPage(int index)
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
                    GoToPage(EditorsHandler.GetPageView(index - 1));
                }
                catch (ArgumentOutOfRangeException)
                {
                    GoToPage(null);
                }
            }
        }

        internal void ChangeWindowIconState(bool state)
        {
            if (state)
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

        private void OnStaticAllDoneChanged(object sender, EventArgs e)
        {
            bool done = WaiterHelper.GetWaiterStatus();
            ProgressBarVisibility = done ? Visibility.Collapsed : Visibility.Visible;
            CheckAllDoneVisibility = done ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
