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
using System.Windows.Media;

namespace cumcad.ViewModels.Handlers
{
    internal class FlipViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private bool isHChecked;
        public bool IsHChecked
        {
            get { return isHChecked; }
            set { SetProperty(ref isHChecked, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private bool isVChecked;
        public bool IsVChecked
        {
            get { return isVChecked; }
            set { SetProperty(ref isVChecked, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public FlipViewModel()
        {
            IsVChecked = true;
            IsHChecked = true;
        }

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    Cv2.CopyTo(image, mat);
                    if (IsHChecked)
                    {
                        var m = mat.Flip(FlipMode.Y);
                        Funcad.ReleaseMat(mat);
                        mat = m;
                    }
                    if (IsVChecked)
                    {
                        var m = mat.Flip(FlipMode.X);
                        Funcad.ReleaseMat(mat);
                        mat = m;
                    }

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
                Params = string.Join(";", new int[] { IsHChecked ? 1 : 0, IsVChecked ? 1 : 0 }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            IsHChecked = items[0] == "1";
            IsVChecked = items[1] == "1";
        }
    }
}
