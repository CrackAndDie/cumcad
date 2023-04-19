using cumcad.Models;
using cumcad.Models.Classes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.ViewModels
{
    internal class EditorPageViewModel : BindableBase
    {
        readonly EditorModel editorModel;

        private EditorItem selectedBranch;
        public EditorItem SelectedBranch
        {
            get { return selectedBranch; }
            set { SetProperty(ref selectedBranch, value); }
        }

        internal event EventHandler<EventArgs> RemoveFromInside;

        public ObservableCollection<EditorItem> TreeViewItems => editorModel.EditorItems;

        internal EditorPageViewModel(SelectEditorResult parameter)
        {
            editorModel = new EditorModel(parameter);
            editorModel.OnRemove += (s, a) => { OnRemove(); RemoveFromInside?.Invoke(this, a); };
        }

        internal void OnRemove()
        {

        }
    }
}
