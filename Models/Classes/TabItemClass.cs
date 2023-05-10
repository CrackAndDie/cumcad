using cumcad.Models.Other;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace cumcad.Models.Classes
{
    internal class TabItemClass
    {
        public PackIconKind IconKind { get; set; }
        public SolidColorBrush IconColor { get; set; }
        public ICommand DeleteCommand { get; set; }

        internal event EventHandler<EventArgs> OnRemove;

        internal TabItemClass()
        {
            DeleteCommand = new DelegateCommand(OnDeleteCommand);
        }

        private void OnDeleteCommand(object parameter)
        {
            // Debug.WriteLine("Should be deleted");
            OnRemove?.Invoke(this, EventArgs.Empty);
        }
    }
}
