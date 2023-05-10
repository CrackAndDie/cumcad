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

namespace cumcad.ViewModels.Handlers
{
    internal class BitwiseNotViewModel : BindableBase, IHandler
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        public event EventHandler<EventArgs> PropertiesChanged;

        public List<Mat> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            foreach (var image in images)
            {
                try
                {
                    Mat mat = new Mat();
                    Cv2.BitwiseNot(image, mat);
                    mats.Add(mat);
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

        public void Selected()
        {

        }

        public void UnSelected()
        {

        }
    }
}
