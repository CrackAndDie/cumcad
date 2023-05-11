using cumcad.ViewModels;
using cumcad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cumcad.Models.Factories
{
    internal class MessageBoxFactory
    {
        internal const string INFO_LOGO = "info.png";
        internal const string WARN_LOGO = "alert.png";
        internal const string QUESTION_LOGO = "question.png";

        internal const int OK_TYPE = 0;
        internal const int YES_NO_TYPE = 1;

        internal static void Show(string msg)
        {
            Show(msg, MessageBoxFactory.WARN_LOGO);
        }

        internal static void Show(string msg, string image)
        {
            Show(msg, image, MessageBoxFactory.OK_TYPE);
        }

        internal static void Show(string msg, string image, int buttonsType)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MyMessageBoxView mb = new MyMessageBoxView();
                // not mvvm way
                mb.Owner = System.Windows.Application.Current.MainWindow;
                var vm = new MyMessageBoxViewModel(msg, image, buttonsType);
                mb.DataContext = vm;
                mb.ShowDialog();
            });
        }

        async internal static Task<bool> ShowAsync(string msg, string image, int buttonsType)
        {
            var vm = new MyMessageBoxViewModel(msg, image, buttonsType);
            Application.Current.Dispatcher.Invoke(() =>
            {
                MyMessageBoxView mb = new MyMessageBoxView();
                // not mvvm way
                mb.Owner = System.Windows.Application.Current.MainWindow;
                mb.DataContext = vm;
                mb.ShowDialog();
            });
            return await vm.GetResult();
        }
    }
}
