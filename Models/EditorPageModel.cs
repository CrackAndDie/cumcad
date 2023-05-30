using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.Models.Other.MyEventArgs;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace cumcad.Models
{
    internal class EditorPageModel : BindableBase, ISaveable
    {
        internal EditorPageModel ParentEditorModel { get; set; }
        internal SelectEditorResult EditorResult { get; set; }
        public ObservableCollection<EditorItem> EditorItems { get; }
        internal int EditorIndex { get; set; }

        internal Mat BeforeImage;

        private BitmapImage viewedImage;
        public BitmapImage ViewedImage
        {
            get { return viewedImage; }
            set { SetProperty(ref viewedImage, value); }
        }

        private EditorItem lastSelectedItem;
        private bool canBeAffected = true;

        // editor remove because of some problems in mainhandler
        internal event EventHandler<EventArgs> OnRemove;

        internal EditorPageModel(SelectEditorResult editorResult)
        {
            EditorIndex = editorResult.EditorIndex;
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
        internal async void ChangeImageSelection(EditorItem item)
        {
            if (canBeAffected)
            {
                canBeAffected = false;
                WaiterHelper.AddWaiter();
                if (lastSelectedItem != null)
                {
                    Funcad.GetIHandler(lastSelectedItem).PropertiesChanged -= OnHandlerPropertiesChanged;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Funcad.GetIHandler(lastSelectedItem).UnSelected();
                    });
                }
                if (BeforeImage != null)
                {
                    Funcad.ReleaseMat(BeforeImage);
                }
                var handler = Funcad.GetIHandler(item);
                lastSelectedItem = item;
                handler.PropertiesChanged += OnHandlerPropertiesChanged;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    handler.Selected();
                });
                int index = IndexOf(item);
                // getting the first images
                if (GetDataContext(0) is MainHandlerViewModel)
                {
                    var mat = await GetUpTo(item);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (ViewedImage != null)
                        {
                            // ViewedImage.Clear();
                        }
                        try
                        {
                            ViewedImage = Funcad.FromMatToBitmap(mat);
                        }
                        catch (OpenCVException ex)
                        {
                            MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                            MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                        }
                    });
                    Funcad.ReleaseMat(mat);
                }
                WaiterHelper.RemoveWaiter();
                canBeAffected = true;
            }
        }

        private async void OnHandlerPropertiesChanged(object sender, EventArgs args)
        {
            if (canBeAffected)
            {
                canBeAffected = false;
                if (lastSelectedItem != null)
                {
                    WaiterHelper.AddWaiter();
                    var mat = await Funcad.GetIHandler(lastSelectedItem).GetResult(BeforeImage);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (ViewedImage != null)
                        {
                            // ViewedImage.Clear();
                        }
                        try
                        {
                            ViewedImage = Funcad.FromMatToBitmap(mat);
                        }
                        catch (OpenCVException ex)
                        {
                            MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                            MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                        }
                    });
                    Funcad.ReleaseMat(mat);
                    WaiterHelper.RemoveWaiter();
                }
                canBeAffected = true;
            }
        }

        internal async Task<Mat> GetUpTo(EditorItem item)
        {
            WaiterHelper.AddWaiter();
            int index = IndexOf(item);
            BeforeImage = await Get(0).GetResult(null);
            if (BeforeImage != null)
            {
                for (int i = 1; i < index; i++)
                {
                    var result = await Get(i).GetResult(BeforeImage);
                    Funcad.ReleaseMat(BeforeImage);
                    BeforeImage = result;
                }
                var mat = await Get(index).GetResult(BeforeImage);
                WaiterHelper.RemoveWaiter();
                GC.Collect();
                return mat;
            }
            WaiterHelper.RemoveWaiter();
            GC.Collect();
            return null;
        }

        internal async Task<Mat> GetUpToQuiet(EditorItem item)
        {
            WaiterHelper.AddWaiter();
            int index = IndexOf(item);
            var image = await Get(0).GetResult(null);
            if (image != null)
            {
                for (int i = 1; i <= index; i++)
                {
                    var result = await Get(i).GetResult(image);
                    Funcad.ReleaseMat(image);
                    image = result;
                }
                WaiterHelper.RemoveWaiter();
                GC.Collect();
                return image;
            }
            WaiterHelper.RemoveWaiter();
            GC.Collect();
            return null;
        }
        #endregion

        internal int IndexOf(EditorItem item)
        {
            return EditorItems.IndexOf(item);
        }

        internal int IndexOf(IHandler item)
        {
            return EditorItems.Select(x => x.Controls[0].SettingsContent.DataContext as IHandler).ToList().IndexOf(item);
        }

        internal IHandler Get(int ind)
        {
            return Funcad.GetIHandler(EditorItems[ind]);
        }

        internal EditorItem GetEditorItem(int ind)
        {
            return EditorItems[ind];
        }

        internal object GetDataContext(int ind)
        {
            return EditorItems[ind].Controls[0].SettingsContent.DataContext;
        }

        internal ObservableCollection<EditorItem> GetItems()
        {
            return EditorItems;
        }

        internal List<HandlerSaveableClass> GetHandlerSaveableObjects()
        {
            // except MainHandler so there is a Skip
            return EditorItems.Select(x => (x.Controls[0].SettingsContent.DataContext as ISaveable).GetSaveableObject() as HandlerSaveableClass).Skip(1).ToList();
        }

        internal List<ISaveable> GetHandlerSaveableInterfaces()
        {
            // except MainHandler so there is a Skip
            return EditorItems.Select(x => x.Controls[0].SettingsContent.DataContext as ISaveable).Skip(1).ToList();
        }

        internal EditorItem Add(int index)
        {
            var type = HandlerFactory.HandlerItems[index];
            return Add(type);
        }

        internal EditorItem Add(string realName)
        {
            var type = HandlerFactory.HandlerItems.FirstOrDefault(x => x.RealName == realName);
            return Add(type);
        }

        internal EditorItem Add(HandlerType type)
        {
            var item = HandlerFactory.GetHandler(type);
            if (item == null)
                return null;
            return Add(item, type);
        }

        internal EditorItem Add(UserControl item, HandlerType type)
        {
            (item.DataContext as IHandler).HandlerEditorModel = this;
            var handler = new EditorItem(item)
            {
                Name = type.Name,
            };
            EditorItems.Add(handler);
            handler.WantsToBeMoved += OnItemWantsToBeMoved;
            return handler;
        }

        internal void Remove(EditorItem item)
        {
            var ind = EditorItems.IndexOf(item);
            if (ind != 0)
            {
                Funcad.GetIHandler(item).OnRemove();
                item.WantsToBeMoved -= OnItemWantsToBeMoved;
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
                EditorItems[0].WantsToBeMoved -= OnItemWantsToBeMoved;
                EditorItems.RemoveAt(0);
            }
        }

        private void OnItemWantsToBeMoved(object sender, MoveDirectionEventArgs args)
        {
            var dir = args.Parameter;
            var item = sender as EditorItem;
            var ind = IndexOf(item);
            if (dir == MoveDirection.Up)
            {
                if (ind == 1)
                    return;
                var swapperItem = EditorItems[ind - 1];
                EditorItems[ind - 1] = item;
                EditorItems[ind] = swapperItem;
            }
            else if (dir == MoveDirection.Down)
            {
                if (ind == EditorItems.Count - 1)
                    return;
                var swapperItem = EditorItems[ind + 1];
                EditorItems[ind + 1] = item;
                EditorItems[ind] = swapperItem;
            }
        }

        public object GetSaveableObject()
        {
            int parentInd = EditorResult.ParentEditorModel != null ? EditorsHandler.IndexOf(EditorResult.ParentEditorModel) : -1;
            return new EditorSaveableClass()
            {
                HandlerItems = GetHandlerSaveableObjects(),
                EditorIndex = EditorIndex,
                IconColor = EditorResult.IconColor,
                ImagePath = EditorResult.ImagePath,
                SelectedType = EditorResult.SelectedType,
                ParentEditorModelIndex = parentInd,
                ParentEditorItemIndex = parentInd >= 0 ? parentInd : -1,
            };
        }

        public void SetSaveableObject(object obj)
        {
            // should be done
        }
    }
}
