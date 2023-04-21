using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace cumcad.Models.Helpers
{
    internal class Funcad
    {
        public static ObservableCollection<BitmapImage> FromMatToBitmap(List<Mat> mats)
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

        public static void ReleaseMats(List<Mat> mats)
        {
            foreach (Mat mat in mats)
            {
                mat.Release();
                mat.Dispose();
            }
            mats.Clear();
            GC.Collect();
        }
    }
}
