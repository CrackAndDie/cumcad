using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using cumcad.Models;
using OpenCvSharp;

namespace cumcad.ViewModels.Base
{
    internal interface IHandler
    {
        EditorPageModel HandlerEditorModel { get; set; }

        void Selected();
        void UnSelected();
        Task<Mat> GetResult(Mat image);
        void OnRemove();
        event EventHandler<EventArgs> PropertiesChanged;
    }
}
