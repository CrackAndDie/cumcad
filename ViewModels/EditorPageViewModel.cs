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
        internal readonly EditorModel editorModel;

        private EditorItem lastSelectedItem;
        private EditorItem selectedBranch;
        public EditorItem SelectedBranch
        {
            get { return selectedBranch; }
            set { SetProperty(ref selectedBranch, value); OnSelectChanged(selectedBranch); }
        }

        private ObservableCollection<BitmapImage> viewedImages;
        public ObservableCollection<BitmapImage> ViewedImages
        {
            get { return viewedImages; }
            set { SetProperty(ref viewedImages, value); }
        }

        private int ugRows;
        public int UGRows
        {
            get { return ugRows; }
            set { SetProperty(ref ugRows, value); }
        }

        private int ugColumns;
        public int UGColumns
        {
            get { return ugColumns; }
            set { SetProperty(ref ugColumns, value); }
        }

        internal event EventHandler<EventArgs> RemoveFromInside;
        internal event EventHandler<EditorItemEventArgs> CreateFromEditorItem;

        public ObservableCollection<EditorItem> TreeViewItems => editorModel.EditorItems;

        #region Commands
        public ICommand AddCommand { get; set; }
        #endregion

        internal EditorPageViewModel(SelectEditorResult parameter)
        {
            AddCommand = new DelegateCommand(OnAddCommand);

            editorModel = new EditorModel(parameter);
            editorModel.OnRemove += (s, a) => { OnRemove(); RemoveFromInside?.Invoke(this, a); };
        }

        async private void OnAddCommand(object parameter)
        {
            var selectedIndex = await SelectorsFactory.OpenSelectHandlerWindow();
            if (selectedIndex != null)
            {
                var uc = HandlerFactory.GetHandler(HandlerFactory.StringItems[(int)selectedIndex]);
                if (uc != null)
                {
                    var item = editorModel.Add(uc, HandlerFactory.StringItems[(int)selectedIndex]);
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
            if (lastSelectedItem != null)
            {
                EditorModel.GetIHandler(lastSelectedItem).PropertiesChanged -= OnHandlerPropertiesChanged;
                EditorModel.GetIHandler(lastSelectedItem).UnSelected();
            }
            if (editorModel.BeforeImages != null)
            {
                Funcad.ReleaseMats(editorModel.BeforeImages);
            }
            var handler = EditorModel.GetIHandler(item);
            handler.PropertiesChanged += OnHandlerPropertiesChanged;
            handler.Selected();
            int index = editorModel.IndexOf(item);
            // getting the first images
            if (editorModel.GetDataContext(0) is MainHandlerViewModel)
            {
                var mats = editorModel.GetUpTo(item);
                if (mats != null)
                {
                    if (ViewedImages != null)
                    {
                        ViewedImages.Clear();
                    }
                    ReCalcUG(mats.Count);
                    ViewedImages = Funcad.FromMatToBitmap(mats);
                    Funcad.ReleaseMats(mats);
                    lastSelectedItem = item;
                }
            }
        }

        private void OnHandlerPropertiesChanged(object sender, EventArgs args)
        {
            var mats = EditorModel.GetIHandler(lastSelectedItem).GetResult(editorModel.BeforeImages);
            if (ViewedImages != null)
            {
                ViewedImages.Clear();
            }
            ReCalcUG(mats.Count);
            ViewedImages = Funcad.FromMatToBitmap(mats);
            Funcad.ReleaseMats(mats);
        }

        private void ReCalcUG(int amount)
        {
            double sqrt = Math.Sqrt(amount);
            int ceiled = (int)Math.Ceiling(sqrt);

            UGColumns = ceiled;
            UGRows = (int)Math.Ceiling(amount / (float)ceiled);
        }

        // inside method that is called when the page is going to be removed
        internal void OnRemove()
        {
            editorModel.RemoveAll();
            if (editorModel.BeforeImages != null)
            {
                Funcad.ReleaseMats(editorModel.BeforeImages);
            }
        }
    }
}
