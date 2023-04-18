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
        readonly EditorModel editorModel = new EditorModel();

        private EditorItem selectedBranch;
        public EditorItem SelectedBranch
        {
            get { return selectedBranch; }
            set { SetProperty(ref selectedBranch, value); }
        }

        public ObservableCollection<EditorItem> TreeViewItems => editorModel.EditorItems;

        internal EditorPageViewModel(SelectEditorResult parameter)
        {
            editorModel.Test();
        }

        internal void OnRemove()
        {

        }
    }
}
