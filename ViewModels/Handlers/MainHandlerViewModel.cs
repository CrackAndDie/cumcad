using cumcad.Models.Classes;
using cumcad.Models.Factories;
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
    internal class MainHandlerViewModel : BindableBase, IHandler
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        // this is for camera or shufflecad
        internal event EventHandler<EventArgs> ImageUpdate;
        // if there is a problem, we should close the page and remove it
        internal event EventHandler<EventArgs> ShouldBeKilled;

        private SelectEditorResult editorData;

        private Mat image;

        internal MainHandlerViewModel(SelectEditorResult editorData)
        {
            Name = editorData.SelectedType.ToString();
            this.editorData = editorData;

            SetUp();
        }

        async private void SetUp()
        {
            // cringe :)
            // this done because we should wait parents to be subscribed to our ShouldBeKilled event
            await Task.Run(() =>
            {
                while (ShouldBeKilled == null) ;
            });
            if (editorData.SelectedType == EditorType.Image)
            {
                try
                {
                    image = new Mat(editorData.ImagePath, ImreadModes.Color);
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Show("F*ck your image, this is a shite. The next MessageBox is going to show you the error", MessageBoxFactory.WARN_LOGO);
                    MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                    ShouldBeKilled?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public List<Mat> GetResult(List<Mat> images)
        {
            if (editorData.SelectedType == EditorType.Image)
            {
                return new List<Mat> { image };
            }
            return new List<Mat>();
        }
    }
}
