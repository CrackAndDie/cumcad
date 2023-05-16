using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.Models.Helpers
{
    internal class Viscad
    {
        internal static Mat RejectBorders(Mat src, bool[] sides, int step = 1)
        {
            Mat outImage = new Mat();
            src.CopyTo(outImage);
            for (int i = 0; i < src.Rows; i += step)
            {
                if (sides[3] && src.Get<Vec3b>(i, 0).Item0 == 255)
                {
                    Cv2.FloodFill(outImage, new Point(0, i), 0);
                }
                if (sides[1] && src.Get<Vec3b>(i, src.Cols - 1).Item0 == 255)
                {
                    Cv2.FloodFill(outImage, new Point(src.Cols - 1, i), 0);
                }
            }
            for (int i = 0; i < src.Cols; i += step)
            {
                if (sides[0] && src.Get<Vec3b>(0, i).Item0 == 255)
                {
                    Cv2.FloodFill(outImage, new Point(i, 0), 0);
                }
                if (sides[2] && src.Get<Vec3b>(src.Rows - 1, i).Item0 == 255)
                {
                    Cv2.FloodFill(outImage, new Point(i, src.Rows - 1), 0);
                }
            }
            return outImage;
        }

        internal static Mat RotateImage(Mat src, int angle)
        {
            int h = src.Rows;
            int w = src.Cols;
            int cx = w / 2;
            int cy = h / 2;
            var m = Cv2.GetRotationMatrix2D(new Point2f(cx, cy), angle, 1.0);
            var cos = Math.Abs(m.Get<double>(0, 0));
            var sin = Math.Abs(m.Get<double>(0, 1));
            int nw = (int)(h * sin + w * cos);
            int nh = (int)(h * cos + w * sin);
            m.Set<double>(0, 2, (nw / 2.0 - cx) + m.Get<double>(0, 2));
            m.Set<double>(1, 2, (nh / 2.0 - cy) + m.Get<double>(1, 2));
            Mat dst = new Mat();
            Cv2.WarpAffine(src, dst, m, new Size(nw, nh));
            m.Release();
            m.Dispose();
            return dst;
        }
    }
}
