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
            set { SetProperty(ref redLowerValue, value); }
        }

        private int redHigherValue;
        public int RedHigherValue
        {
            get { return redHigherValue; }
            set { SetProperty(ref redHigherValue, value); }
        }

        private int greenLowerValue;
        public int GreenLowerValue
        {
            get { return greenLowerValue; }
            set { SetProperty(ref greenLowerValue, value); }
        }

        private int greenHigherValue;
        public int GreenHigherValue
        {
            get { return greenHigherValue; }
            set { SetProperty(ref greenHigherValue, value); }
        }

        private int blueLowerValue;
        public int BlueLowerValue
        {
            get { return blueLowerValue; }
            set { SetProperty(ref blueLowerValue, value); }
        }

        private int blueHigherValue;
        public int BlueHigherValue
        {
            get { return blueHigherValue; }
            set { SetProperty(ref blueHigherValue, value); }
        }

        public List<Mat> GetResult(List<Mat> images)
        {
            throw new NotImplementedException();
        }
    }
}
