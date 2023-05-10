using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.ViewModels;
using cumcad.ViewModels.Base;
using cumcad.ViewModels.Handlers;
using cumcad.Views.Handlers;
using OpenCvSharp;
using OpenCvSharp.Flann;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace cumcad.Models
{
    internal class EditorPageModel : BindableBase
    {
        internal EditorPageModel ParentEditorModel { get; set; }
        internal SelectEditorResult EditorResult { get; set; }
        public ObservableCollection<EditorItem> EditorItems { get; }

        internal List<Mat> BeforeImages;

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

        private EditorItem lastSelectedItem;

        // editor remove because of some problems in mainhandler
        internal event EventHandler<EventArgs> OnRemove;

        internal EditorPageModel(SelectEditorResult editorResult)
        {
            EditorResult = editorResult;
            EditorItems = new ObservableCollection<EditorItem>();
            ParentEditorModel = editorResult.ParentEditorModel;
            AddMainHandler();
        }

        private void AddMainHandler()
        {
            var mhView = new MainHandlerView();
            var mhViewModel = new MainHandlerViewModel(EditorResult);
            mhViewModel.ShouldBeKilled += (s, a) => { OnRemove?.Invoke(s, a); };
            mhView.DataContext = mhViewModel;
            var mainHandler = new EditorItem(mhView)
            {
                Name = EditorResult.SelectedType.ToString(),
            };
            EditorItems.Add(mainHandler);
        }

        #region HighLevel
        internal void ChangeImageSelection(EditorItem item)
        {
            if (lastSelectedItem != null)
            {
                Funcad.GetIHandler(lastSelectedItem).PropertiesChanged -= OnHandlerPropertiesChanged;
                Funcad.GetIHandler(lastSelectedItem).UnSelected();
            }
            if (BeforeImages != null)
            {
                Funcad.ReleaseMats(BeforeImages);
            }
            var handler = Funcad.GetIHandler(item);
            handler.PropertiesChanged += OnHandlerPropertiesChanged;
            handler.Selected();
            int index = IndexOf(item);
            // getting the first images
            if (GetDataContext(0) is MainHandlerViewModel)
            {
                var mats = GetUpTo(item);
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
            var mats = Funcad.GetIHandler(lastSelectedItem).GetResult(BeforeImages);
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

        internal List<Mat> GetUpTo(EditorItem item)
        {
            int index = IndexOf(item);
            BeforeImages = Get(0).GetResult(null);
            if (BeforeImages != null)
            {
                for (int i = 1; i < index; i++)
                {
                    var result = Get(i).GetResult(BeforeImages);
                    Funcad.ReleaseMats(BeforeImages);
                    BeforeImages = result;
                }
                var mats = Get(index).GetResult(BeforeImages);
                return mats;
            }
            return null;
        }

        internal List<Mat> GetUpToQuiet(EditorItem item)
        {
            int index = IndexOf(item);
            var images = Get(0).GetResult(null);
            if (images != null)
            {
                for (int i = 1; i <= index; i++)
                {
                    var result = Get(i).GetResult(images);
                    Funcad.ReleaseMats(images);
                    images = result;
                }
                return images;
            }
            return null;
        }
        #endregion

        internal int IndexOf(EditorItem item)
        {
            return EditorItems.IndexOf(item);
        }

        internal IHandler Get(int ind)
        {
            return Funcad.GetIHandler(EditorItems[ind]);
        }

        internal object GetDataContext(int ind)
        {
            return EditorItems[ind].Controls[0].SettingsContent.DataContext;
        }

        internal ObservableCollection<EditorItem> GetItems()
        {
            return EditorItems;
        }

        internal EditorItem Add(int index)
        {
            var name = HandlerFactory.StringItems[index];
            var item = HandlerFactory.GetHandler(name);
            if (item == null)
                return null;
            var handler = new EditorItem(item)
            {
                Name = name,
            };
            EditorItems.Add(handler);
            return handler;
        }

        internal EditorItem Add(UserControl item, string name)
        {
            var handler = new EditorItem(item)
            {
                Name = name,
            };
            EditorItems.Add(handler);
            return handler;
        }

        internal void Remove(EditorItem item)
        {
            var ind = EditorItems.IndexOf(item);
            if (ind != 0)
            {
                Funcad.GetIHandler(item).OnRemove();
                EditorItems.Remove(item);
            }
            else
            {
                MessageBoxFactory.Show("Remove youself, you're a mess", MessageBoxFactory.INFO_LOGO);
            }
        }

        internal void RemoveAll()
        {
            while (EditorItems.Count > 0)
            {
                Funcad.GetIHandler(EditorItems[0]).OnRemove();
                EditorItems.RemoveAt(0);
            }
        }
    }
}
