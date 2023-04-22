using cumcad.Models.Other;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace cumcad.Models.Classes
{
    internal class EditorInsideItem : BindableBase
    {
        private UserControl settingsContent;
        public UserControl SettingsContent
        {
            get { return settingsContent; }
            set { SetProperty(ref settingsContent, value); }
        }
    }

    internal class EditorItem : BindableBase
    {
        private string name;
        public string Name 
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public ObservableCollection<EditorInsideItem> Controls { get; }

        public ICommand DeleteCommand { get; set; }
        internal event EventHandler<EventArgs> WantsToBeRemoved;

        public ICommand EditorFromThisCommand { get; set; }
        internal event EventHandler<EventArgs> CreateFromThis;

        internal EditorItem(UserControl control)
        {
            DeleteCommand = new DelegateCommand((parameter) => { WantsToBeRemoved?.Invoke(this, EventArgs.Empty); });
            EditorFromThisCommand = new DelegateCommand((parameter) => { CreateFromThis?.Invoke(this, EventArgs.Empty); });

            Controls = new ObservableCollection<EditorInsideItem>();
            Controls.Add(new EditorInsideItem() { SettingsContent = control });
        }
    }
}
