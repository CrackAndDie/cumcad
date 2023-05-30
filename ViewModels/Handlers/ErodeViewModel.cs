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
    internal class ErodeViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private bool freezeEvent = false;

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

        private int iters;
        public int Iters
        {
            get { return iters; }
            set
            {
                SetProperty(ref iters, value);
                if (!freezeEvent)
                    PropertiesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public ErodeViewModel()
        {
            Width = 7;
            Height = 7;
            Iters = 3;
        }

        public async Task<Mat> GetResult(Mat image)
        {
            Application.Current.Dispatcher.Invoke(() => { freezeEvent = true; });
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    var br = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(Width, Height));
                    Cv2.Erode(image, mat, br, iterations: Iters);
                    Funcad.ReleaseMat(br);
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
                Params = string.Join(";", new int[] { Width, Height, Iters }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            Width = int.Parse(items[0]);
            Height = int.Parse(items[1]);
            Height = int.Parse(items[2]);
        }
    }
}
