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
using System.Threading.Tasks;
using System.Windows.Media;

namespace cumcad.ViewModels.Handlers
{
    internal class BitwiseOrAndViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private bool isOrChecked;
        public bool IsOrChecked
        {
            get { return isOrChecked; }
            set { SetProperty(ref isOrChecked, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

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

        private List<Brush> maskEditorBrushes;
        public List<Brush> MaskEditorBrushes
        {
            get { return maskEditorBrushes; }
            set { SetProperty(ref maskEditorBrushes, value); }
        }

        private int selectedMaskEditor;
        public int SelectedMaskEditor
        {
            get { return selectedMaskEditor; }
            set { SetProperty(ref selectedMaskEditor, value); MaskSelectionChanged(selectedMaskEditor); }
        }

        private List<string> maskEditorHandlers;
        public List<string> MaskEditorHandlers
        {
            get { return maskEditorHandlers; }
            set { SetProperty(ref maskEditorHandlers, value); }
        }

        private int selectedMaskHandler;
        public int SelectedMaskHandler
        {
            get { return selectedMaskHandler; }
            set { SetProperty(ref selectedMaskHandler, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public BitwiseOrAndViewModel()
        {
            IsOrChecked = true;
        }

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    var independentModels = EditorsHandler.GetIndependentEditorModels(HandlerEditorModel, this);
                    Mat mask = null;
                    if (SelectedMaskEditor > 0)
                    {
                        var maskEditorModel = independentModels[SelectedMaskEditor - 1];
                        mask = maskEditorModel.GetUpToQuiet(maskEditorModel.GetItems()[SelectedMaskHandler]).GetAwaiter().GetResult();
                    }

                    if (SelectedImg2Editor >= 0 && independentModels.Count > 0)
                    {
                        var img2EditorModel = independentModels[SelectedImg2Editor];
                        Mat img2 = img2EditorModel.GetUpToQuiet(img2EditorModel.GetItems()[SelectedImg2Handler]).GetAwaiter().GetResult();

                        if (IsOrChecked)
                        {
                            Cv2.BitwiseOr(image, img2, mat, mask);
                        }
                        else
                        {
                            Cv2.BitwiseAnd(image, img2, mat, mask);
                        }
                    }
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

        private void Img2SelectionChanged(int index)
        {
            Img2EditorHandlers = GetHandlerNames(index);
        }

        private void MaskSelectionChanged(int index)
        {
            index -= 1;
            if (index >= 0)
                MaskEditorHandlers = GetHandlerNames(index);
            else
                MaskEditorHandlers = null;
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

        private List<Brush> GetMaskBrushes()
        {
            var lst = GetImg2Brushes();
            lst.Insert(0, Funcad.RedStrokeBrush());
            return lst;
        }

        public void Selected()
        {
            Img2EditorBrushes = GetImg2Brushes();
            MaskEditorBrushes = GetMaskBrushes();
            Img2SelectionChanged(SelectedImg2Editor);
            MaskSelectionChanged(SelectedMaskEditor);
        }

        public void UnSelected()
        {
            
        }

        public object GetSaveableObject()
        {
            return new HandlerSaveableClass()
            {
                Name = this.GetType().Name.Substring(0, this.GetType().Name.Length - 9),
                Params = string.Join(";", new int[] { IsOrChecked ? 1 : 0, SelectedImg2Editor, SelectedImg2Handler, SelectedMaskEditor, SelectedMaskHandler }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            IsOrChecked = items[0] == "1";
            SelectedImg2Editor = int.Parse(items[1]);
            SelectedImg2Handler = int.Parse(items[2]);
            SelectedMaskEditor = int.Parse(items[3]);
            SelectedMaskHandler = int.Parse(items[4]);
        }
    }
}
