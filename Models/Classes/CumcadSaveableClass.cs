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
    public class HandlerSaveableClass
    {
        public string Name { get; set; }
        public string Params { get; set; }
    }

    public class EditorSaveableClass
    {
        public int EditorIndex { get; set; }
        public EditorType SelectedType { get; set; }
        public SolidColorBrush IconColor { get; set; }
        // for IMAGE type
        public string ImagePath { get; set; }
        // for BUFFER type
        public byte[] BufferImage { get; set; }
        // for MAT type
        public int NewMatWidth { get; set; }
        public int NewMatHeight { get; set; }
        public int NewMatFill { get; set; }
        public MatType NewMatType { get; set; }
        // for FROMEDITOR type
        public int ParentEditorModelIndex { get; set; }
        public int ParentEditorItemIndex { get; set; }

        public List<HandlerSaveableClass> HandlerItems { get; set; }
    }

    public class CumcadSaveableClass
    {
        public List<EditorSaveableClass> EditorItems { get; set; }
    }
}
