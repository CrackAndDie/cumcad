using cumcad.Models.Classes;
using cumcad.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace cumcad.Views
{
    /// <summary>
    /// Interaction logic for EditorPageView.xaml
    /// </summary>
    public partial class EditorPageView : Page
    {
        public EditorPageView()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = e.NewValue as EditorItem;
            if (item != null)
            {
                (DataContext as EditorPageViewModel).SelectedBranch = item;
            }
        }
    }
}
