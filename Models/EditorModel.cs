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
using static System.Net.Mime.MediaTypeNames;

namespace cumcad.Models
{
    internal class EditorModel : BindableBase
    {
        internal SelectEditorResult EditorResult { get; set; }
        public ObservableCollection<EditorItem> EditorItems { get; }

        internal List<Mat> BeforeImages;

        internal event EventHandler<EventArgs> OnRemove;

        internal EditorModel(SelectEditorResult editorResult)
        {
            EditorResult = editorResult;
            EditorItems = new ObservableCollection<EditorItem>();
            var mhView = new MainHandlerView();
            var mhViewModel = new MainHandlerViewModel(editorResult);
            mhViewModel.ShouldBeKilled += (s, a) => { OnRemove?.Invoke(s, a); };
            mhView.DataContext = mhViewModel;
            var mainHandler = new EditorItem(mhView)
            {
                Name = editorResult.SelectedType.ToString(),
            };
            EditorItems.Add(mainHandler);
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

        internal static IHandler GetIHandler(EditorItem item)
        {
            return item.Controls[0].SettingsContent.DataContext as IHandler;
        }

        internal int IndexOf(EditorItem item)
        {
            return EditorItems.IndexOf(item);
        }

        internal IHandler Get(int ind)
        {
            return GetIHandler(EditorItems[ind]);
        }

        internal ObservableCollection<EditorItem> GetItems()
        {
            return EditorItems;
        }

        internal object GetDataContext(int ind)
        {
            return EditorItems[ind].Controls[0].SettingsContent.DataContext;
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
                GetIHandler(item).OnRemove();
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
                GetIHandler(EditorItems[0]).OnRemove();
                EditorItems.RemoveAt(0);
            }
        }
    }
}
