using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.ViewModels;
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

        internal void Add(UserControl item, string name)
        {
            var handler = new EditorItem(item)
            {
                Name = name,
            };
            EditorItems.Add(handler);
        }

        internal void Remove(EditorItem item)
        {
            var ind = EditorItems.IndexOf(item);
            if (ind != 0)
            {
                EditorItems.Remove(item);
            }
            else
            {
                MessageBoxFactory.Show("Remove youself, you're a mess", MessageBoxFactory.INFO_LOGO);
            }
        }
    }
}
