using cumcad.Models;
using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Other;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        // inside method that is called when the page is going to be removed
        internal void OnRemove()
        {

        }
    }
}
