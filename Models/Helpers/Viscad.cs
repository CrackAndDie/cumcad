using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Media;
using System.Security.Cryptography;

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

        internal static Mat RotateImageSaveShape(Mat src, int angle)
        {
            int h = src.Rows;
            int w = src.Cols;
            int cx = w / 2;
            int cy = h / 2;
            var m = Cv2.GetRotationMatrix2D(new Point2f(cx, cy), angle, 1.0);
            Mat dst = new Mat();
            Cv2.WarpAffine(src, dst, m, new Size(w, h));
            m.Release();
            m.Dispose();
            return dst;
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

        internal static Mat FillHoles(Mat src)
        {
            Mat dst = new Mat(src.Rows, src.Cols, src.Type(), new Scalar(0));
            Cv2.FindContours(src, out Point[][] countours, out HierarchyIndex[] _, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);
            for (int i = 0; i < countours.Length; ++i)
            {
                Cv2.DrawContours(dst, countours, i, new Scalar(255), Cv2.FILLED);
            }
            return dst;
        }

        internal static Point CenterOfMass(Mat src)
        {
            var m = Cv2.Moments(src, true);
            return new Point(m.M10/m.M00, m.M01/m.M00);
        }

        internal static Mat ParticleFilter(Mat src, int power)
        {
            Mat dst = new Mat(src.Rows, src.Cols, src.Type());
            Mat labels = new Mat(), stats = new Mat(), centroids = new Mat();
            int num = Cv2.ConnectedComponentsWithStats(src, labels, stats, centroids, PixelConnectivity.Connectivity8);

            for (int i = 1; i < num; ++i)
            {
                if (stats.Get<int>(i, 4) >= power)
                {
                    // genius thing, I will forget how it works. But the reason is CV_32SC1 type of dst
                    Mat bin = labels.InRange(new Scalar(i), new Scalar(i));
                    Cv2.BitwiseOr(bin, dst, dst);
                    Funcad.ReleaseMat(bin);
                }
            }
            Funcad.ReleaseMat(labels);
            Funcad.ReleaseMat(stats);
            Funcad.ReleaseMat(centroids);
            return dst;
        }

        internal static Point[][] GetSortedBlobs(Mat src)
        {
            Cv2.FindContours(src, out Point[][] countours, out HierarchyIndex[] _, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);
            return countours.ToList().OrderBy(x => Cv2.ContourArea(x)).ToArray();
        }
    }
}
