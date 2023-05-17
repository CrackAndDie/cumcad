using cumcad.Models;
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
using System.Windows;
using System.Windows.Threading;

namespace cumcad.ViewModels.Handlers
{
    internal class ResizeViewModel : BindableBase, IHandler
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private bool freezeEvent = false;
        private bool firstCall = true;

        private int width;
        public int Width
        {
            get { return width; }
            set 
            {
                SetProperty(ref width, value); 
                if (!freezeEvent)
                    PropertiesChanged?.Invoke(this, EventArgs.Empty); 
            }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set
            {
                SetProperty(ref height, value);
                if (!freezeEvent)
                    PropertiesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public async Task<Mat> GetResult(Mat image)
        {
            Application.Current.Dispatcher.Invoke(() => { freezeEvent = true; });
            var mat = new Mat();
            await Task.Run(() =>
            {
                if (firstCall)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Width = image.Width;
                        Height = image.Height;
                    });
                    firstCall = false;
                }

                try
                {
                    Mat m = image.Resize(new OpenCvSharp.Size(Width, Height));
                    Funcad.ReleaseMat(mat);
                    mat = m;
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
    }
}
