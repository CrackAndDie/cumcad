using cumcad.Models;
using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.Models.Other;
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
        readonly EditorModel editorModel;

        private EditorItem lastSelectedItem;
        private EditorItem selectedBranch;
        public EditorItem SelectedBranch
        {
            get { return selectedBranch; }
            set { SetProperty(ref selectedBranch, value); OnSelectChanged(selectedBranch); }
        }

        private List<Mat> beforeImages;
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

        public ObservableCollection<EditorItem> TreeViewItems => editorModel.EditorItems;

        #region Commands
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        #endregion

        internal EditorPageViewModel(SelectEditorResult parameter)
        {
            AddCommand = new DelegateCommand(OnAddCommand);
            RemoveCommand = new DelegateCommand(OnRemoveCommand);

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
                    editorModel.Add(uc, HandlerFactory.StringItems[(int)selectedIndex]);
                }
            }
        }

        private void OnRemoveCommand(object parameter)
        {
            if (SelectedBranch != null)
            {
                editorModel.Remove(SelectedBranch);
            }
        }

        private void OnSelectChanged(EditorItem item)
        {
            if (lastSelectedItem != null)
            {
                EditorModel.GetIHandler(lastSelectedItem).PropertiesChanged -= OnHandlerPropertiesChanged;
            }
            if (beforeImages != null)
            {
                Funcad.ReleaseMats(beforeImages);
            }
            var handler = EditorModel.GetIHandler(item);
            handler.PropertiesChanged += OnHandlerPropertiesChanged;
            int index = editorModel.IndexOf(item);
            // getting the first images
            beforeImages = editorModel.Get(0).GetResult(null);
            for (int i = 1; i < index; i++)
            {
                var result = editorModel.Get(i).GetResult(beforeImages);
                Funcad.ReleaseMats(beforeImages);
                beforeImages = result;            
            }
            var mats = editorModel.Get(index).GetResult(beforeImages);
            if (ViewedImages != null)
            {
                ViewedImages.Clear();
            }
            ReCalcUG(mats.Count);
            ViewedImages = Funcad.FromMatToBitmap(mats);
            Funcad.ReleaseMats(mats);
            lastSelectedItem = item;
        }

        private void OnHandlerPropertiesChanged(object sender, EventArgs args)
        {
            var mats = EditorModel.GetIHandler(lastSelectedItem).GetResult(beforeImages);
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
            if (beforeImages != null)
            {
                Funcad.ReleaseMats(beforeImages);
            }
        }
    }
}
