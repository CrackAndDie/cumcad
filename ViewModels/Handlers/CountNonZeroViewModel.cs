using cumcad.Models;
using cumcad.Models.Classes;
using cumcad.ViewModels.Base;
using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace cumcad.ViewModels.Handlers
{
    internal class CountNonZeroViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private int nonZeroPixels = 0;
        public int NonZeroPixels
        {
            get { return nonZeroPixels; }
            set { SetProperty(ref nonZeroPixels, value); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                image.CopyTo(mat);
                int counted = mat.Channels() == 1 ? mat.CountNonZero() : 0;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NonZeroPixels = counted;
                });
            });
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
                Params = "",
            };
        }

        public void SetSaveableObject(object obj)
        {
            
        }
    }
}
