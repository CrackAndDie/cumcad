using cumcad.Models.Classes;
using cumcad.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cumcad.Models.Helpers
{
    internal class Funcad
    {
        public static ObservableCollection<BitmapImage> FromMatToBitmap(List<OpenCvSharp.Mat> mats)
        {
            var bitmaps = new ObservableCollection<BitmapImage>();
            foreach (var mat in mats)
            {
                using (var ms = mat.ToMemoryStream())
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    bitmaps.Add(bitmap);
                }
            }
            return bitmaps;
        }

        public static void ReleaseMats(List<OpenCvSharp.Mat> mats)
        {
            foreach (OpenCvSharp.Mat mat in mats)
            {
                mat.Release();
                mat.Dispose();
            }
            mats.Clear();
            GC.Collect();
        }

        public static IHandler GetIHandler(EditorItem item)
        {
            return item.Controls[0].SettingsContent.DataContext as IHandler;
        }

        public static SolidColorBrush PickRandomBrush()
        {
            SolidColorBrush result;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (SolidColorBrush)properties[random].GetValue(null, null);

            return result;
        }

        public static LinearGradientBrush RedStrokeBrush()
        {
            LinearGradientBrush myVerticalGradient = new LinearGradientBrush();
            myVerticalGradient.StartPoint = new Point(0.35, 0);
            myVerticalGradient.EndPoint = new Point(0.65, 1);
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.0));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.42));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.5));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.58));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 1.0));
            return myVerticalGradient;
        }
    }
}
