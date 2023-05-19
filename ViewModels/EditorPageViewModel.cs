using cumcad.Models;
using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.Models.Other;
using cumcad.Models.Other.MyEventArgs;
using cumcad.ViewModels.Handlers;
using cumcad.Views.Handlers;
using OpenCvSharp;
using OpenCvSharp.Flann;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace cumcad.ViewModels
{
    internal class EditorPageViewModel : BindableBase
    {
        internal readonly EditorPageModel editorModel;

        private EditorItem selectedBranch;
        public EditorItem SelectedBranch
        {
            get { return selectedBranch; }
            set { SetProperty(ref selectedBranch, value); OnSelectChanged(selectedBranch); }
        }

        public BitmapImage ViewedImage => editorModel.ViewedImage;

        public ObservableCollection<EditorItem> TreeViewItems => editorModel.EditorItems;

        internal event EventHandler<EventArgs> RemoveFromInside;
        internal event EventHandler<EditorItemEventArgs> CreateFromEditorItem;

        #region Commands
        public ICommand AddCommand { get; set; }
        #endregion

        internal EditorPageViewModel(SelectEditorResult parameter)
        {
            AddCommand = new DelegateCommand(OnAddCommand);

            editorModel = new EditorPageModel(parameter);
            editorModel.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            editorModel.OnRemove += (s, a) => { RemoveFromInside?.Invoke(this, a); };
        }

        private void OnAddCommand(object parameter)
        {
            AddHandler();
        }

        async private void AddHandler()
        {
            var selectedIndex = await SelectorsFactory.OpenSelectHandlerWindow();
            if (selectedIndex != null)
            {
                var item = editorModel.Add((int)selectedIndex);
                if (item != null)
                {
                    item.WantsToBeRemoved += OnItemWantsToBeRemoved;
                    item.CreateFromThis += OnWantsToBeCreatedFromEditorItem;
                }
            }
        }

        private void OnWantsToBeCreatedFromEditorItem(object sender, EventArgs args)
        {
            CreateFromEditorItem?.Invoke(editorModel, new EditorItemEventArgs(sender as EditorItem));
        }

        private void OnItemWantsToBeRemoved(object sender, EventArgs args)
        {
            var item = sender as EditorItem;
            if (item != null)
            {
                item.WantsToBeRemoved -= OnItemWantsToBeRemoved;
                item.CreateFromThis -= OnWantsToBeCreatedFromEditorItem;
                editorModel.Remove(item);
            }
        }

        private void OnSelectChanged(EditorItem item)
        {
            editorModel.ChangeImageSelection(item);
        }

        // inside method that is called when the page is going to be removed
        internal void OnRemove()
        {
            editorModel.RemoveAll();
            if (editorModel.BeforeImage != null)
            {
                Funcad.ReleaseMat(editorModel.BeforeImage);
            }
        }
    }
}
