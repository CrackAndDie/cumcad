using cumcad.ViewModels;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace cumcad.Models.Classes
{
    class SelectEditorResult
    {
        internal int EditorIndex { get; set; }
        internal bool? IsSelected { get; set; }
        internal EditorType SelectedType { get; set; }
        internal SolidColorBrush IconColor { get; set; }
        // for IMAGE type
        internal string ImagePath { get; set; }
        // for BUFFER type
        internal Mat BufferImage { get; set; }
        // for MAT type
        internal int NewMatWidth { get; set; }
        internal int NewMatHeight { get; set; }
        internal int NewMatFill { get; set; }
        internal MatType NewMatType { get; set; }
        // for FROMEDITOR type
        internal EditorPageModel ParentEditorModel { get; set; }
        internal EditorItem ParentEditorItem { get; set; }
    }
}
