using cumcad.Models.Classes;
using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.Models;
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
    internal class AutoBrightnessViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private int neededValue;
        public int NeededValue
        {
            get { return neededValue; }
            set { SetProperty(ref neededValue, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private List<Brush> cutEditorBrushes;
        public List<Brush> CutEditorBrushes
        {
            get { return cutEditorBrushes; }
            set { SetProperty(ref cutEditorBrushes, value); }
        }

        private int selectedCutEditor;
        public int SelectedCutEditor
        {
            get { return selectedCutEditor; }
            set { SetProperty(ref selectedCutEditor, value); CutSelectionChanged(selectedCutEditor); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private List<string> cutEditorHandlers;
        public List<string> CutEditorHandlers
        {
            get { return cutEditorHandlers; }
            set { SetProperty(ref cutEditorHandlers, value); }
        }

        private int selectedCutHandler;
        public int SelectedCutHandler
        {
            get { return selectedCutHandler; }
            set { SetProperty(ref selectedCutHandler, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public AutoBrightnessViewModel()
        {
            NeededValue = 100;
        }

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    var independentModels = EditorsHandler.GetIndependentEditorModels(HandlerEditorModel, this);

                    if (SelectedCutEditor >= 0 && independentModels.Count > 0)
                    {
                        var img2EditorModel = independentModels[SelectedCutEditor];
                        Mat img2 = img2EditorModel.GetUpToQuiet(img2EditorModel.GetItems()[SelectedCutHandler]).GetAwaiter().GetResult();
                        
                        Funcad.ReleaseMat(mat);
                        mat = Viscad.AutoBrightness(img2, image, NeededValue);
                        Funcad.ReleaseMat(img2);
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

        private void CutSelectionChanged(int index)
        {
            CutEditorHandlers = GetHandlerNames(index);
        }

        private List<string> GetHandlerNames(int index)
        {
            var items = EditorsHandler.GetIndependentEditorModels(HandlerEditorModel, this);
            if (items.Count <= 0)
                return new List<string>();
            return items[index].GetItems().Select(x => x.Name).ToList();
        }

        private List<Brush> GetCutBrushes()
        {
            return EditorsHandler.GetIndependentEditorModels(HandlerEditorModel, this).Select(x => x.EditorResult.IconColor as Brush).ToList();
        }

        public void OnRemove()
        {

        }

        public void Selected()
        {
            CutEditorBrushes = GetCutBrushes();
            CutSelectionChanged(SelectedCutEditor);
        }

        public void UnSelected()
        {

        }

        public object GetSaveableObject()
        {
            return new HandlerSaveableClass()
            {
                Name = this.GetType().Name.Substring(0, this.GetType().Name.Length - 9),
                Params = string.Join(";", new int[] { NeededValue, SelectedCutEditor, SelectedCutHandler }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            NeededValue = int.Parse(items[0]);
            SelectedCutEditor = int.Parse(items[1]);
            SelectedCutHandler = int.Parse(items[2]);
        }
    }
}
