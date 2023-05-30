using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.Models;
using cumcad.ViewModels.Base;
using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cumcad.ViewModels.Handlers
{
    internal class CannyViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private bool freezeEvent = false;

        private int minValue;
        public int MinValue
        {
            get { return minValue; }
            set
            {
                SetProperty(ref minValue, value);
                if (!freezeEvent)
                    PropertiesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private int maxValue;
        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                SetProperty(ref maxValue, value);
                if (!freezeEvent)
                    PropertiesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public CannyViewModel()
        {
            MinValue = 1;
            MaxValue = 10;
        }

        public async Task<Mat> GetResult(Mat image)
        {
            Application.Current.Dispatcher.Invoke(() => { freezeEvent = true; });
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    Cv2.Canny(image, mat, MinValue, MaxValue);
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                    MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                }
            });
            Application.Current.Dispatcher.Invoke(() => { freezeEvent = false; });
            return mat;
        }

        public void OnRemove()
        {

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
                Params = string.Join(";", new int[] { MinValue, MaxValue }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            MinValue = int.Parse(items[0]);
            MaxValue = int.Parse(items[1]);
        }
    }
}
