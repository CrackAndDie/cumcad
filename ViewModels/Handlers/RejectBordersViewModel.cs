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
    internal class RejectBordersViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private bool isTopChecked;
        public bool IsTopChecked
        {
            get { return isTopChecked; }
            set { SetProperty(ref isTopChecked, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private bool isRightChecked;
        public bool IsRightChecked
        {
            get { return isRightChecked; }
            set { SetProperty(ref isRightChecked, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private bool isBottomChecked;
        public bool IsBottomChecked
        {
            get { return isBottomChecked; }
            set { SetProperty(ref isBottomChecked, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private bool isLeftChecked;
        public bool IsLeftChecked
        {
            get { return isLeftChecked; }
            set { SetProperty(ref isLeftChecked, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private int currentFastStep;
        public int CurrentFastStep
        {
            get { return currentFastStep; }
            set { SetProperty(ref currentFastStep, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public RejectBordersViewModel()
        {
            IsTopChecked = true;
            IsRightChecked = true;
            IsBottomChecked = true;
            IsLeftChecked = true;
        }

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    Mat m = Viscad.RejectBorders(image,
                        new bool[] { IsTopChecked, IsRightChecked, IsBottomChecked, IsLeftChecked },
                        CurrentFastStep >= 1 ? CurrentFastStep : 1);
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
                Params = string.Join(";", new int[] { IsTopChecked ? 1 : 0, IsRightChecked ? 1 : 0, IsBottomChecked ? 1 : 0, IsLeftChecked ? 1 : 0, CurrentFastStep}),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            IsTopChecked = items[0] == "1";
            IsRightChecked = items[1] == "1";
            IsBottomChecked = items[2] == "1";
            IsLeftChecked = items[3] == "1";
            CurrentFastStep = int.Parse(items[4]);
        }
    }
}
