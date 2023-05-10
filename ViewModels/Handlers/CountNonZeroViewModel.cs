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

        public async  Task<List<Mat>> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            NonZeroPixels.Clear();
            await Task.Run(() =>
            {
                foreach (var image in images)
                {
                    Mat newMat = new Mat();
                    image.CopyTo(newMat);
                    mats.Add(newMat);
                    int counted = newMat.Channels() == 1 ? newMat.CountNonZero() : 0;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NonZeroPixels.Add(counted);
                    });
                }
            });
            return mats;
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
