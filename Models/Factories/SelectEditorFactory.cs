using cumcad.ViewModels;
using cumcad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.Models.Factories
{
    internal class SelectEditorFactory
    {
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
    }
}
