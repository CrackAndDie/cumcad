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

namespace cumcad.ViewModels.Handlers
{
    internal class RotateImageViewModel : BindableBase, IHandler
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

        public async Task<List<Mat>> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            await Task.Run(() =>
            {
                foreach (var image in images)
                {
                    try
                    {
                        mats.Add(IsSaveShapeChecked ? Viscad.RotateImageSaveShape(image, CurrentAngle) : Viscad.RotateImage(image, CurrentAngle));
                    }
                    catch (Exception ex)
                    {
                        MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                        MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                    }
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
