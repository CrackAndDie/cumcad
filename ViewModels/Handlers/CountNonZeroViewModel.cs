using cumcad.Models;
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
    internal class CountNonZeroViewModel : BindableBase, IHandler
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private ObservableCollection<int> nonZeroPixels = new ObservableCollection<int>();
        public ObservableCollection<int> NonZeroPixels
        {
            get { return nonZeroPixels; }
            set
            {
                SetProperty(ref nonZeroPixels, value);
            }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            Application.Current.Dispatcher.Invoke(() =>
            {
                NonZeroPixels.Clear();
            });
            await Task.Run(() =>
            {
                image.CopyTo(mat);
                int counted = mat.Channels() == 1 ? mat.CountNonZero() : 0;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NonZeroPixels.Add(counted);
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
    }
}
