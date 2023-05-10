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

namespace cumcad.ViewModels.Handlers
{
    internal class InRangeGrayViewModel : BindableBase, IHandler
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private int grayLowerValue;
        public int GrayLowerValue
        {
            get { return grayLowerValue; }
            set { SetProperty(ref grayLowerValue, value); NotSmartEventCalled(); }
        }

        private int grayHigherValue;
        public int GrayHigherValue
        {
            get { return grayHigherValue; }
            set { SetProperty(ref grayHigherValue, value); NotSmartEventCalled(); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public List<Mat> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            foreach (var image in images)
            {
                try
                {
                    mats.Add(image.InRange(InputArray.Create(new int[] { GrayLowerValue }),
                        InputArray.Create(new int[] { GrayHigherValue })));
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                    MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                }
            }
            return mats;
        }

        public void OnRemove()
        {
            
        }

        private DateTime lastCallTime = DateTime.Now;
        private void NotSmartEventCalled()
        {
            if ((DateTime.Now - lastCallTime).TotalMilliseconds > 100)
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
