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
        internal static List<string> StringItems = new List<string>() { "ExtractImage", "InRange", "InRangeGray", "Resize", "CvtColor", "CountNonZero", "BitwiseNot", "Anime" };

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

        private static UserControl GetExtractImage()
        {
            var view = new ExtractImageView();
            var vm = new ExtractImageViewModel();
            view.DataContext = vm;
            return view;
        }

        private static UserControl GetInRange()
        {
            var view = new InRangeView();
            var vm = new InRangeViewModel();
            view.DataContext = vm;
            return view;
        }

        private static UserControl GetInRangeGray()
        {
            var view = new InRangeGrayView();
            var vm = new InRangeGrayViewModel();
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

        private static UserControl GetCvtColor()
        {
            var view = new CvtColorView();
            var vm = new CvtColorViewModel();
            view.DataContext = vm;
            return view;
        }

        private static UserControl GetCountNonZero()
        {
            var view = new CountNonZeroView();
            var vm = new CountNonZeroViewModel();
            view.DataContext = vm;
            return view;
        }

        private static UserControl GetBitwiseNot()
        {
            var view = new BitwiseNotView();
            var vm = new BitwiseNotViewModel();
            view.DataContext = vm;
            return view;
        }
    }
}
