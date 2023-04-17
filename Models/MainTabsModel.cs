using MaterialDesignThemes.Wpf;
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
    internal class TabItemClass
    {
        public PackIconKind IconKind { get; set; }
    }

    internal class MainTabsModel : BindableBase
    {
        private readonly ObservableCollection<TabItemClass> tabItems = new ObservableCollection<TabItemClass>();
        internal readonly ReadOnlyObservableCollection<TabItemClass> TabItems;

        internal MainTabsModel() 
        {
            TabItems = new ReadOnlyObservableCollection<TabItemClass>(tabItems);
            tabItems.Add(new TabItemClass() { IconKind = PackIconKind.PaperRollOutline });
        }

        internal void AddNewItem()
        {
            tabItems.Add(new TabItemClass() { IconKind = PackIconKind.Karate });
        }
    }
}
