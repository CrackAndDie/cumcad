using cumcad.Models;
using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.ViewModels.Base;
using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace cumcad.ViewModels.Handlers
{
    internal class ExtractImageViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private int xStartValue;
        public int XStartValue
        {
            get { return xStartValue; }
            set { SetProperty(ref xStartValue, value); NotSmartEventCalled(); }
        }

        private int xStopValue;
        public int XStopValue
        {
            get { return xStopValue; }
            set { SetProperty(ref xStopValue, value); NotSmartEventCalled(); }
        }

        private int yStartValue;
        public int YStartValue
        {
            get { return yStartValue; }
            set { SetProperty(ref yStartValue, value); NotSmartEventCalled(); }
        }

        private int yStopValue;
        public int YStopValue
        {
            get { return yStopValue; }
            set { SetProperty(ref yStopValue, value); NotSmartEventCalled(); }
        }

        private int imageWidth;
        public int ImageWidth
        {
            get { return imageWidth; }
            set { SetProperty(ref imageWidth, value); }
        }

        private int imageHeight;
        public int ImageHeight
        {
            get { return imageHeight; }
            set { SetProperty(ref imageHeight, value); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ImageWidth = image.Width;
                    ImageHeight = image.Height;
                });

                try
                {
                    Mat m = image.SubMat(YStartValue, YStopValue - 1, XStartValue, XStopValue - 1);
                    Funcad.ReleaseMat(mat);
                    mat = m;
                }
                catch (Exception)
                {
                    // this error appears when the image is initialized, so there is no need to show the problem
                }
            });
            return mat;
        }

        public void OnRemove()
        {
            
        }

        private DateTime lastCallTime = DateTime.Now;
        private void NotSmartEventCalled()
        {
            if ((DateTime.Now - lastCallTime).TotalMilliseconds > 200)
            {
                lastCallTime = DateTime.Now;
                PropertiesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Selected()
        {
            
        }

        public void UnSelected()
        {
            
        }

        public object GetSaveableObject()
        {
            return new HandlerSaveableClass()
            {
                Name = this.GetType().Name.Substring(0, this.GetType().Name.Length - 9),
                Params = string.Join(";", new int[] { XStartValue, XStopValue, YStartValue, YStopValue, ImageWidth, ImageHeight }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            XStartValue = int.Parse(items[0]);
            XStopValue = int.Parse(items[1]);
            YStartValue = int.Parse(items[2]);
            YStopValue = int.Parse(items[3]);
            ImageWidth = int.Parse(items[4]);
            ImageHeight = int.Parse(items[5]);
        }
    }
}
