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
using System.Windows.Media;

namespace cumcad.ViewModels.Handlers
{
    internal class MergeViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private List<Brush> img2EditorBrushes;
        public List<Brush> Img2EditorBrushes
        {
            get { return img2EditorBrushes; }
            set { SetProperty(ref img2EditorBrushes, value); }
        }

        private int selectedImg2Editor;
        public int SelectedImg2Editor
        {
            get { return selectedImg2Editor; }
            set { SetProperty(ref selectedImg2Editor, value); Img2SelectionChanged(selectedImg2Editor); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
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

        private List<Brush> img3EditorBrushes;
        public List<Brush> Img3EditorBrushes
        {
            get { return img3EditorBrushes; }
            set { SetProperty(ref img3EditorBrushes, value); }
        }

        private int selectedImg3Editor;
        public int SelectedImg3Editor
        {
            get { return selectedImg3Editor; }
            set { SetProperty(ref selectedImg3Editor, value); Img3SelectionChanged(selectedImg3Editor); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private List<string> img3EditorHandlers;
        public List<string> Img3EditorHandlers
        {
            get { return img3EditorHandlers; }
            set { SetProperty(ref img3EditorHandlers, value); }
        }

        private int selectedImg3Handler;
        public int SelectedImg3Handler
        {
            get { return selectedImg3Handler; }
            set { SetProperty(ref selectedImg3Handler, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    var independentModels = EditorsHandler.GetIndependentEditorModels(HandlerEditorModel, this);
                    Mat img3 = null;
                    if (SelectedImg3Editor > 0 && independentModels.Count > 0)
                    {
                        var maskEditorModel = independentModels[SelectedImg3Editor - 1];
                        img3 = maskEditorModel.GetUpToQuiet(maskEditorModel.GetItems()[SelectedImg3Handler]).GetAwaiter().GetResult();
                    }

                    Mat img2 = null;
                    if (SelectedImg2Editor >= 0 && independentModels.Count > 0)
                    {
                        var img2EditorModel = independentModels[SelectedImg2Editor];
                        img2 = img2EditorModel.GetUpToQuiet(img2EditorModel.GetItems()[SelectedImg2Handler]).GetAwaiter().GetResult();

                        if (img3 != null)
                        {
                            Cv2.Merge(new Mat[] { image, img2, img3 }, mat);
                        }
                        else
                        {
                            Cv2.Merge(new Mat[] { image, img2 }, mat);
                        }
                    }

                    if (img3 != null)
                        Funcad.ReleaseMat(img3);
                    if (img2 != null)
                        Funcad.ReleaseMat(img2);
                }
                catch (Exception ex)
                {
                    MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                    MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                }
            });
            return mat;
        }

        private void Img2SelectionChanged(int index)
        {
            Img2EditorHandlers = GetHandlerNames(index);
        }

        private void Img3SelectionChanged(int index)
        {
            index -= 1;
            if (index >= 0)
                Img3EditorHandlers = GetHandlerNames(index);
            else
                Img3EditorHandlers = null;
        }

        private List<string> GetHandlerNames(int index)
        {
            var items = EditorsHandler.GetIndependentEditorModels(HandlerEditorModel, this);
            if (items.Count <= 0)
                return new List<string>();
            return items[index].GetItems().Select(x => x.Name).ToList();
        }

        private List<Brush> GetImg2Brushes()
        {
            return EditorsHandler.GetIndependentEditorModels(HandlerEditorModel, this).Select(x => x.EditorResult.IconColor as Brush).ToList();
        }

        private List<Brush> GetImg3Brushes()
        {
            var lst = GetImg2Brushes();
            lst.Insert(0, Funcad.RedStrokeBrush());
            return lst;
        }

        public void OnRemove()
        {
            
        }

        public void Selected()
        {
            Img2EditorBrushes = GetImg2Brushes();
            Img3EditorBrushes = GetImg3Brushes();
            Img2SelectionChanged(SelectedImg2Editor);
            Img3SelectionChanged(SelectedImg3Editor);
        }

        public void UnSelected()
        {
            
        }

        public object GetSaveableObject()
        {
            return new HandlerSaveableClass()
            {
                Name = this.GetType().Name.Substring(0, this.GetType().Name.Length - 9),
                Params = string.Join(";", new int[] { SelectedImg2Editor, SelectedImg2Handler, SelectedImg3Editor, SelectedImg3Handler }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            SelectedImg2Editor = int.Parse(items[0]);
            SelectedImg2Handler = int.Parse(items[1]);
            SelectedImg3Editor = int.Parse(items[2]);
            SelectedImg3Handler = int.Parse(items[3]);
        }
    }
}
