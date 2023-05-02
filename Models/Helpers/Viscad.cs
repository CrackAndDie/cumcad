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
        internal static Mat RejectBorders(Mat src, bool[] sides)
        {
            Mat outImage = new Mat();
            src.CopyTo(outImage);
            for (int i = 0; i < src.Rows; i++)
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
            for (int i = 0; i < src.Cols; i++)
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
    }
}
