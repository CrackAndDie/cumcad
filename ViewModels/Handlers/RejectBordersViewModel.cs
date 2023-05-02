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
    internal class RejectBordersViewModel : BindableBase, IHandler
    {
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

        public event EventHandler<EventArgs> PropertiesChanged;

        internal RejectBordersViewModel()
        {
            IsTopChecked = true;
            IsRightChecked = true;
            IsBottomChecked = true;
            IsLeftChecked = true;
        }

        public List<Mat> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            foreach (var image in images)
            {
                try
                {
                    mats.Add(Viscad.RejectBorders(image, new bool[] { IsTopChecked, IsRightChecked, IsBottomChecked, IsLeftChecked }));
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                    MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                }
            }
            return mats;
        }

        public void OnRemove()
        {
            
        }
    }
}
