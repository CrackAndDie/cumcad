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
using System.Windows;

namespace cumcad.ViewModels.Handlers
{
    internal class BlobViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private int currentBlob;
        public int CurrentBlob
        {
            get { return currentBlob; }
            set { SetProperty(ref currentBlob, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        private int blobAmount;
        public int BlobAmount
        {
            get { return blobAmount; }
            set { SetProperty(ref blobAmount, value); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public BlobViewModel()
        {
            BlobAmount = 1;
        }

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat(image.Rows, image.Cols, image.Type(), new Scalar(0));
            await Task.Run(() =>
            {
                try
                {
                    var points = Viscad.GetSortedBlobs(image);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        BlobAmount = points.Length;
                    });
                    Cv2.DrawContours(mat, points, CurrentBlob - 1, new Scalar(255), -1);
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

        public void Selected()
        {

        }

        public void UnSelected()
        {

        }

        public object GetSaveableObject()
        {
            return new HandlerSaveableClass()
            {
                Name = this.GetType().Name.Substring(0, this.GetType().Name.Length - 9),
                Params = string.Join(";", new int[] { CurrentBlob, BlobAmount }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            BlobAmount = int.Parse(items[1]);
            CurrentBlob = int.Parse(items[0]);
        }
    }
}
