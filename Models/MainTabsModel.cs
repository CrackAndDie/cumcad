using cumcad.Models.Classes;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace cumcad.Models
{
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
