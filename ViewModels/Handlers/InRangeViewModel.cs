using cumcad.Models.Classes;
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
        private int redLowerValue;
        public int RedLowerValue
        {
            get { return redLowerValue; }
            set { SetProperty(ref redLowerValue, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private int redHigherValue;
        public int RedHigherValue
        {
            get { return redHigherValue; }
            set { SetProperty(ref redHigherValue, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private int greenLowerValue;
        public int GreenLowerValue
        {
            get { return greenLowerValue; }
            set { SetProperty(ref greenLowerValue, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private int greenHigherValue;
        public int GreenHigherValue
        {
            get { return greenHigherValue; }
            set { SetProperty(ref greenHigherValue, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private int blueLowerValue;
        public int BlueLowerValue
        {
            get { return blueLowerValue; }
            set { SetProperty(ref blueLowerValue, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private int blueHigherValue;
        public int BlueHigherValue
        {
            get { return blueHigherValue; }
            set { SetProperty(ref blueHigherValue, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public List<Mat> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            foreach (var image in images)
            {
                mats.Add(image.InRange(InputArray.Create(new int[] { RedLowerValue, GreenLowerValue, BlueLowerValue }), 
                    InputArray.Create(new int[] { RedHigherValue, GreenHigherValue, BlueHigherValue })));
                mats.Add(image.InRange(InputArray.Create(new int[] { RedLowerValue, GreenLowerValue, BlueLowerValue }),
                    InputArray.Create(new int[] { RedHigherValue, GreenHigherValue, BlueHigherValue })));
                mats.Add(image.InRange(InputArray.Create(new int[] { RedLowerValue, GreenLowerValue, BlueLowerValue }),
                    InputArray.Create(new int[] { RedHigherValue, GreenHigherValue, BlueHigherValue })));
            }
            return mats;
        }
    }
}
