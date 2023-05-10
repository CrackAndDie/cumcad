using cumcad.Models;
using cumcad.Models.Classes;
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
    internal class InRangeViewModel : BindableBase, IHandler
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private int redLowerValue;
        public int RedLowerValue
        {
            get { return redLowerValue; }
            set { SetProperty(ref redLowerValue, value); NotSmartEventCalled(); }
        }

        private int redHigherValue;
        public int RedHigherValue
        {
            get { return redHigherValue; }
            set { SetProperty(ref redHigherValue, value); NotSmartEventCalled(); }
        }

        private int greenLowerValue;
        public int GreenLowerValue
        {
            get { return greenLowerValue; }
            set { SetProperty(ref greenLowerValue, value); NotSmartEventCalled(); }
        }

        private int greenHigherValue;
        public int GreenHigherValue
        {
            get { return greenHigherValue; }
            set { SetProperty(ref greenHigherValue, value); NotSmartEventCalled(); }
        }

        private int blueLowerValue;
        public int BlueLowerValue
        {
            get { return blueLowerValue; }
            set { SetProperty(ref blueLowerValue, value); NotSmartEventCalled(); }
        }

        private int blueHigherValue;
        public int BlueHigherValue
        {
            get { return blueHigherValue; }
            set { SetProperty(ref blueHigherValue, value); NotSmartEventCalled(); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public List<Mat> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            foreach (var image in images)
            {
                try
                {
                    mats.Add(image.InRange(InputArray.Create(new int[] { RedLowerValue, GreenLowerValue, BlueLowerValue }),
                        InputArray.Create(new int[] { RedHigherValue, GreenHigherValue, BlueHigherValue })));
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
