using cumcad.Models.Classes;
using cumcad.ViewModels;
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
            tabItems.Add(new TabItemClass()
            {
                IconKind = PackIconKind.PaperRollOutline,
                IconColor = new SolidColorBrush(Colors.AliceBlue),
                ParentIconColor = new SolidColorBrush(Colors.Transparent),
            });
        }

        internal TabItemClass AddNewItem(SelectEditorResult result)
        {
            var item = new TabItemClass()
            {
                IconKind = PackIconKind.ImageEditOutline,
                IconColor = result.IconColor,
                ParentIconColor = result.ParentEditorModel == null ? new SolidColorBrush(Colors.Transparent) : result.ParentEditorModel.EditorResult.IconColor,
            };
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
