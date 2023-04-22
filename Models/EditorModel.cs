using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.ViewModels;
using cumcad.ViewModels.Base;
using cumcad.ViewModels.Handlers;
using cumcad.Views.Handlers;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace cumcad.Models
{
    internal class EditorModel : BindableBase
    {
        public ObservableCollection<EditorItem> EditorItems { get; }

        internal event EventHandler<EventArgs> OnRemove;

        internal EditorModel(SelectEditorResult editorResult)
        {
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
