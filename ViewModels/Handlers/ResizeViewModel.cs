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

        public async Task<List<Mat>> GetResult(List<Mat> images)
        {
            freezeEvent = true;
            var mats = new List<Mat>();
            await Task.Run(() =>
            {
                if (images.Count > 0)
                {
                    if (firstCall)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var size = images[0].Size();
                            Width = size.Width;
                            Height = size.Height;
                        });
                        firstCall = false;
                    }

                    foreach (var image in images)
                    {
                        try
                        {
                            mats.Add(image.Resize(new OpenCvSharp.Size(Width, Height)));
                        }
                        catch (Exception ex)
                        {
                            MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                            MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                        }
                    }
                }
            });
            freezeEvent = false;
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
