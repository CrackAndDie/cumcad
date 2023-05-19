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
    internal class RotateImageViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private int currentAngle;
        public int CurrentAngle
        {
            get { return currentAngle; }
            set { SetProperty(ref currentAngle, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private bool isSaveShapeChecked;
        public bool IsSaveShapeChecked
        {
            get { return isSaveShapeChecked; }
            set { SetProperty(ref isSaveShapeChecked, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    Mat m = IsSaveShapeChecked ? Viscad.RotateImageSaveShape(image, CurrentAngle) : Viscad.RotateImage(image, CurrentAngle);
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
                Params = string.Join(";", new int[] { IsSaveShapeChecked ? 1 : 0, CurrentAngle }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            IsSaveShapeChecked = items[0] == "1";
            CurrentAngle = int.Parse(items[1]);
        }
    }
}
