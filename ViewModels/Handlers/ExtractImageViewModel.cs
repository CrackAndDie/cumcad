using cumcad.Models;
using cumcad.Models.Factories;
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
    internal class ExtractImageViewModel : BindableBase, IHandler
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

        public async Task<List<Mat>> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            await Task.Run(() =>
            {
                if (images.Count > 0)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ImageWidth = images[0].Width;
                        ImageHeight = images[0].Height;
                    });
                    
                    foreach (var image in images)
                    {
                        try
                        {
                            mats.Add(image.SubMat(YStartValue, YStopValue - 1, XStartValue, XStopValue - 1));
                        }
                        catch (Exception)
                        {
                            // this error appears when the image is initialized, so there is no need to show the problem
                        }
                    }
                }
            });
            return mats;
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
    }
}
