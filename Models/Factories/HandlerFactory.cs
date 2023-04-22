using cumcad.ViewModels.Handlers;
using cumcad.Views.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace cumcad.Models.Factories
{
    internal class HandlerFactory
    {
        internal static List<string> StringItems = new List<string>() { "InRange", "Resize", "Anime" };

        internal static UserControl GetHandler(string name)
        {
            try
            {
                Type type = typeof(HandlerFactory);
                MethodInfo methodInfo = type.GetMethod("Get" + name, BindingFlags.NonPublic | BindingFlags.Static);
                if (methodInfo != null)
                {
                    return methodInfo.Invoke(name, null) as UserControl;
                }
            }
            catch (Exception ex)
            {
                // there is no that method....
                Debug.WriteLine(ex);
            }
            return null;
        }

        private static UserControl GetInRange()
        {
            var view = new InRangeView();
            var vm = new InRangeViewModel();
            view.DataContext = vm;
            return view;
        }

        private static UserControl GetResize()
        {
            var view = new ResizeView();
            var vm = new ResizeViewModel();
            view.DataContext = vm;
            return view;
        }
    }
}
