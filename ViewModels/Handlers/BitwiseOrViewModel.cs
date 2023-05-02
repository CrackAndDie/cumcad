using cumcad.Models.Helpers;
using cumcad.ViewModels.Base;
using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace cumcad.ViewModels.Handlers
{
    internal class BitwiseOrViewModel : BindableBase, IHandler
    {
        private List<SolidColorBrush> img2EditorBrushes;
        public List<SolidColorBrush> Img2EditorBrushes
        {
            get { return img2EditorBrushes; }
            set { SetProperty(ref img2EditorBrushes, value); }
        }

        private int selectedImg2Editor;
        public int SelectedImg2Editor
        {
            get { return selectedImg2Editor; }
            set { SetProperty(ref selectedImg2Editor, value); Img2SelectionChanged(selectedImg2Editor); }
        }

        private List<string> img2EditorHandlers;
        public List<string> Img2EditorHandlers
        {
            get { return img2EditorHandlers; }
            set { SetProperty(ref img2EditorHandlers, value); }
        }

        private int selectedImg2Handler;
        public int SelectedImg2Handler
        {
            get { return selectedImg2Handler; }
            set { SetProperty(ref selectedImg2Handler, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        internal BitwiseOrViewModel()
        {
            Img2EditorBrushes = EditorsHelper.GetEditors().Select(x => (x.DataContext as EditorPageViewModel).editorModel.EditorResult.IconColor).ToList();
            SelectedImg2Editor = 0;
        }

        public List<Mat> GetResult(List<Mat> images)
        {
            return new List<Mat>();
        }

        public void OnRemove()
        {
            
        }

        private void Img2SelectionChanged(int index)
        {
            Img2EditorHandlers = (EditorsHelper.GetPageView(index).DataContext as EditorPageViewModel).editorModel.GetItems().Select(x => x.Name).ToList();
        }
    }
}
