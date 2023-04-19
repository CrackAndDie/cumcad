using cumcad.Models.Classes;
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

        //internal void Test()
        //{
        //    var item1 = new EditorItem(new InRangeView())
        //    {
        //        Name = "Test",
        //    };
        //    EditorItems.Add(item1);
        //    var item2 = new EditorItem(new InRangeView())
        //    {
        //        Name = "Test2",
        //    };
        //    EditorItems.Add(item2);
        //}
    }
}
