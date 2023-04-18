using cumcad.Models.Classes;
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

        internal EditorModel()
        {
            EditorItems = new ObservableCollection<EditorItem>();
        }

        internal void Test()
        {
            var item1 = new EditorItem(new InRangeView())
            {
                Name = "Test",
            };
            EditorItems.Add(item1);
            var item2 = new EditorItem(new InRangeView())
            {
                Name = "Test2",
            };
            EditorItems.Add(item2);
        }
    }
}
