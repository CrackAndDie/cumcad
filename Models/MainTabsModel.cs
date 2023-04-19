using cumcad.Models.Other;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace cumcad.Models
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

    internal class MainTabsModel : BindableBase
    {
        private readonly ObservableCollection<TabItemClass> tabItems = new ObservableCollection<TabItemClass>();
        internal readonly ReadOnlyObservableCollection<TabItemClass> TabItems;

        internal MainTabsModel() 
        {
            TabItems = new ReadOnlyObservableCollection<TabItemClass>(tabItems);
            tabItems.Add(new TabItemClass() { IconKind = PackIconKind.PaperRollOutline, IconColor = new SolidColorBrush(Colors.AliceBlue) });
        }

        internal TabItemClass AddNewItem(SolidColorBrush iconColor)
        {
            var item = new TabItemClass() { IconKind = PackIconKind.ImageEditOutline, IconColor = iconColor };
            tabItems.Add(item);
            return item;
        }

        internal int IndexOf(TabItemClass item)
        {
            return tabItems.IndexOf(item);
        }

        internal void RemoveItem(int index)
        {
            tabItems.RemoveAt(index);
        }
    }
}
