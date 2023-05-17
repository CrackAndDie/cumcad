using cumcad.Models.Classes;
using cumcad.ViewModels.Base;
using OpenCvSharp.Internal.Vectors;
using OpenCvSharp.Internal;
using OpenCvSharp;
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
using System.IO;

namespace cumcad.Models.Helpers
{
    internal class Funcad
    {
        public static BitmapImage FromMatToBitmap(Mat mat)
        {
            if (mat.Cols == 0 || mat.Rows == 0)
            {
                return null;
            }
            var bitmap = new BitmapImage();
            using (var ms = ToMemoryStream(mat))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            return bitmap;
        }

        public static void ReleaseMat(Mat mat)
        {
            mat.Release();
            mat.Dispose();
        }

        public static bool ImEncode(string ext, InputArray img, out byte[] buf, int[] prms = null)
        {
            if (string.IsNullOrEmpty(ext))
            {
                throw new ArgumentNullException("ext");
            }

            if (img == null)
            {
                throw new ArgumentNullException("img");
            }

            if (prms == null)
            {
                prms = Array.Empty<int>();
            }

            img.ThrowIfDisposed();
            VectorOfByte vectorOfByte = new VectorOfByte();
            NativeMethods.HandleException(NativeMethods.imgcodecs_imencode_vector(ext, img.CvPtr, vectorOfByte.CvPtr, prms, 0, out var returnValue));
            GC.KeepAlive(img);
            buf = vectorOfByte.ToArray();
            vectorOfByte.Dispose();
            return returnValue != 0;
        }

        public static MemoryStream ToMemoryStream(Mat mat)
        {
            ImEncode(".png", mat, out var buf);
            return new MemoryStream(buf);
        }

        public static IHandler GetIHandler(EditorItem item)
        {
            IHandler handler = null;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                handler = item.Controls[0].SettingsContent.DataContext as IHandler;
            });
            return handler;
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
            myVerticalGradient.StartPoint = new System.Windows.Point(0.35, 0);
            myVerticalGradient.EndPoint = new System.Windows.Point(0.65, 1);
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.0));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.42));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.5));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.58));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 1.0));
            return myVerticalGradient;
        }
    }
}
