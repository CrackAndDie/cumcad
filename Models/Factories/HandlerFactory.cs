using cumcad.Models.Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace cumcad.Models.Factories
{
    internal class HandlerFactory
    {
        internal static List<HandlerType> HandlerItems = new List<HandlerType>()
        {
            new HandlerType() { Name = "CenterOfMass", RealName = "CenterOfMass", Icon = PackIconKind.ImageFilterCenterFocusWeak, },
            new HandlerType() { Name = "Concat", RealName = "Concat", Icon = PackIconKind.PlusBoxMultipleOutline, },
            new HandlerType() { Name = "ExtractImage", RealName = "ExtractImage", Icon = PackIconKind.BoxCutter, },
            new HandlerType() { Name = "GaussianBlur", RealName = "GaussianBlur", Icon = PackIconKind.Blur, },
            new HandlerType() { Name = "InRange", RealName = "InRange", Icon = PackIconKind.Tune, },
            new HandlerType() { Name = "InRangeGray", RealName = "InRangeGray", Icon = PackIconKind.Tune, },
            new HandlerType() { Name = "FillHoles (Slow)", RealName = "FillHoles", Icon = PackIconKind.FormatColorFill, },
            new HandlerType() { Name = "Merge", RealName = "Merge", Icon = PackIconKind.CallMerge, },
            new HandlerType() { Name = "ParticleFilter", RealName = "ParticleFilter", Icon = PackIconKind.FilterMinusOutline, },
            new HandlerType() { Name = "Resize", RealName = "Resize", Icon = PackIconKind.Resize, },
            new HandlerType() { Name = "RejectBorders (Slow)", RealName = "RejectBorders", Icon = PackIconKind.BorderNoneVariant, },
            new HandlerType() { Name = "RotateImage", RealName = "RotateImage", Icon = PackIconKind.CropRotate, },
            new HandlerType() { Name = "CvtColor", RealName = "CvtColor", Icon = PackIconKind.GenderTransgender, },
            new HandlerType() { Name = "CountNonZero", RealName = "CountNonZero", Icon = PackIconKind.Counter, },
            new HandlerType() { Name = "BitwiseNot", RealName = "BitwiseNot", Icon = PackIconKind.GateNot, },
            new HandlerType() { Name = "BitwiseOr/And", RealName = "BitwiseOrAnd", Icon = PackIconKind.GateBuffer, },
            new HandlerType() { Name = "Split", RealName = "Split", Icon = PackIconKind.CallSplit, },
            new HandlerType() { Name = "Transpose", RealName = "Transpose", Icon = PackIconKind.AlphaTBoxOutline, },
            new HandlerType() { Name = "Canny", RealName = "Canny", Icon = PackIconKind.LandslideOutline, },
            new HandlerType() { Name = "Dilate", RealName = "Dilate", Icon = PackIconKind.ArrowExpandHorizontal, },
            new HandlerType() { Name = "Erode", RealName = "Erode", Icon = PackIconKind.FormatHorizontalAlignCenter, },
            new HandlerType() { Name = "Flip", RealName = "Flip", Icon = PackIconKind.Mirror, },
            new HandlerType() { Name = "Blob", RealName = "Blob", Icon = PackIconKind.LiquidSpot, },
        };

        static HandlerFactory()
        {
            HandlerItems = HandlerItems.OrderBy(x => x.Name).ToList();
        }

        internal static UserControl GetHandler(HandlerType type)
        {
            try
            {
                string realName = type.RealName;
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
