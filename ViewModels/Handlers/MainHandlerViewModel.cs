using cumcad.Models;
using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.ViewModels.Base;
using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.CodeDom;
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

        // this is for camera, fromEditor or shufflecad
        public event EventHandler<EventArgs> PropertiesChanged;
        // if there is a problem, we should close the page and remove it
        internal event EventHandler<EventArgs> ShouldBeKilled;

        private readonly SelectEditorResult editorData;

        private Mat image;

        public MainHandlerViewModel(SelectEditorResult editorData)
        {
            Name = editorData.SelectedType.ToString();
            this.editorData = editorData;

            if (editorData.SelectedType == EditorType.FromEditor)
            {
                var handler = editorData.ParentEditorModel.Get(0);
                handler.PropertiesChanged += OnParentPropertiesChanged;
            }
        }

        public List<Mat> GetResult(List<Mat> images)
        {
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
                return new List<Mat> { image };
            }
            else if (editorData.SelectedType == EditorType.FromEditor)
            {
                int index = editorData.ParentEditorModel.IndexOf(editorData.ParentEditorItem);
                if (index >= 0)
                {
                    var mats = editorData.ParentEditorModel.GetUpToQuiet(editorData.ParentEditorItem);
                    return mats;
                }
                else
                {
                    MessageBoxFactory.Show("F*ck your image, this is a shite, you're a dickhead", MessageBoxFactory.WARN_LOGO);
                    ShouldBeKilled?.Invoke(this, EventArgs.Empty);
                }
            }
            return new List<Mat>();
        }

        public void OnRemove()
        {
            if (editorData.SelectedType == EditorType.FromEditor)
            {
                editorData.ParentEditorModel.Get(0).PropertiesChanged -= OnParentPropertiesChanged;
            }
        }

        // for FROMEDITOR
        private void OnParentPropertiesChanged(object sender, EventArgs args)
        {
            PropertiesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Selected()
        {
            
        }

        public void UnSelected()
        {
            
        }
    }
}
