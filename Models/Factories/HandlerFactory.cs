using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace cumcad.Models.Factories
{
    internal class HandlerFactory
    {
        // viewed list
        internal static List<string> StringItems = new List<string>() { "ExtractImage", "InRange", "InRangeGray", "Resize", "RejectBorders", "CvtColor", "CountNonZero", "BitwiseNot", "BitwiseOr" };
        // real names
        internal static List<string> StringItemsRealNames = new List<string>() { "ExtractImage", "InRange", "InRangeGray", "Resize", "RejectBorders", "CvtColor", "CountNonZero", "BitwiseNot", "BitwiseOr" };

        internal static UserControl GetHandler(string name)
        {
            try
            {
                string realName = StringItemsRealNames[StringItems.IndexOf(name)];
                var viewType = Assembly.GetExecutingAssembly().GetType("cumcad.Views.Handlers." + realName + "View");
                var view = (UserControl)Activator.CreateInstance(viewType);
                var vmType = Assembly.GetExecutingAssembly().GetType("cumcad.ViewModels.Handlers." + realName + "ViewModel");
                view.DataContext = Activator.CreateInstance(vmType);
                return view;
            }
            catch (Exception)
            {
                // idk if there could be an error
            }
            return null;
        }
    }
}
