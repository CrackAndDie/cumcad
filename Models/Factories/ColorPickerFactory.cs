using cumcad.ViewModels;
using cumcad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace cumcad.Models.Factories
{
    internal class ColorPickerFactory
    {
        async internal static Task<SolidColorBrush> OpenColorPickerWindow()
        {
            var view = new ColorPickerWindowView();
            // not mvvm way :)
            view.Owner = System.Windows.Application.Current.MainWindow;
            var vm = new ColorPickerWindowViewModel();
            view.DataContext = vm;
            view.ShowDialog();
            return await vm.GetSelectionTask();
        }
    }
}
