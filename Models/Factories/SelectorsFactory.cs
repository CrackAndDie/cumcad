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
    internal class SelectorsFactory
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

        async internal static Task<SelectEditorResult> OpenSelectEditorWindow()
        {
            var view = new SelectEditorWindowView();
            // not mvvm way :)
            view.Owner = System.Windows.Application.Current.MainWindow;
            var vm = new SelectEditorWindowViewModel();
            view.DataContext = vm;
            view.ShowDialog();
            return await vm.GetSelectionTask();
        }

        async internal static Task<int?> OpenSelectHandlerWindow()
        {
            var view = new SelectHandlerWindowView();
            // not mvvm way :)
            view.Owner = System.Windows.Application.Current.MainWindow;
            var vm = new SelectHandlerWindowViewModel();
            view.DataContext = vm;
            view.ShowDialog();
            return await vm.GetSelectionTask();
        }
    }
}
