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

namespace cumcad.ViewModels.Handlers
{
    internal class InRangeViewModel : BindableBase, IHandler, ISaveable
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

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    Mat m = image.InRange(InputArray.Create(new int[] { RedLowerValue, GreenLowerValue, BlueLowerValue }),
                        InputArray.Create(new int[] { RedHigherValue, GreenHigherValue, BlueHigherValue }));
                    Funcad.ReleaseMat(mat);
                    mat = m;
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                    MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
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

        public object GetSaveableObject()
        {
            return new HandlerSaveableClass()
            {
                Name = this.GetType().Name.Substring(0, this.GetType().Name.Length - 9),
                Params = string.Join(";", new int[] { RedLowerValue, RedHigherValue, GreenLowerValue, GreenHigherValue, BlueLowerValue, BlueHigherValue }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            RedLowerValue = int.Parse(items[0]);
            RedHigherValue = int.Parse(items[1]);
            GreenLowerValue = int.Parse(items[2]);
            GreenHigherValue = int.Parse(items[3]);
            BlueLowerValue = int.Parse(items[4]);
            BlueHigherValue = int.Parse(items[5]);
        }
    }
}
