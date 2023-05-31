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
using System.Windows;
using System.Windows.Threading;

namespace cumcad.ViewModels.Handlers
{
    internal class MainHandlerViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

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

        private Mat imageFromFile;

        public MainHandlerViewModel(SelectEditorResult editorData)
        {
            Name = editorData.SelectedType.ToString();
            this.editorData = editorData;

            if (editorData.SelectedType == EditorType.Image)
            {
                imageFromFile = new Mat(editorData.ImagePath, ImreadModes.Color);
            }
            else if (editorData.SelectedType == EditorType.FromEditor)
            {
                var handler = editorData.ParentEditorModel.Get(0);
                handler.PropertiesChanged += OnParentPropertiesChanged;
            }
        }

        public async Task<Mat> GetResult(Mat image)
        {
            Mat result = null;
            await Task.Run(() =>
            {
                if (editorData.SelectedType == EditorType.Image)
                {
                    try
                    {
                        Mat dst = new Mat();
                        imageFromFile.CopyTo(dst);
                        result = dst;
                    }
                    catch (Exception ex)
                    {
                        MessageBoxFactory.Show("F*ck your image, this is a shite. The next MessageBox is going to show you the error", MessageBoxFactory.WARN_LOGO);
                        MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                        // im not sure that I should do this using Dispatcher Invoke
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ShouldBeKilled?.Invoke(this, EventArgs.Empty);
                        });
                    }
                }
                else if (editorData.SelectedType == EditorType.Buffer)
                {
                    try
                    {
                        Mat dst = new Mat();
                        editorData.BufferImage.CopyTo(dst);
                        result = dst;
                    }
                    catch (Exception ex)
                    {
                        MessageBoxFactory.Show("F*ck your image, this is a shite. The next MessageBox is going to show you the error", MessageBoxFactory.WARN_LOGO);
                        MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                        // im not sure that I should do this using Dispatcher Invoke
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ShouldBeKilled?.Invoke(this, EventArgs.Empty);
                        });
                    }
                }
                else if (editorData.SelectedType == EditorType.FromEditor)
                {
                    int index = editorData.ParentEditorModel.IndexOf(editorData.ParentEditorItem);
                    if (index >= 0)
                    {
                        result = editorData.ParentEditorModel.GetUpToQuiet(editorData.ParentEditorItem).GetAwaiter().GetResult();
                    }
                    else
                    {
                        MessageBoxFactory.Show("F*ck your image, this is a shite, you're a dickhead", MessageBoxFactory.WARN_LOGO);
                        // im not sure that I should do this using Dispatcher Invoke
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ShouldBeKilled?.Invoke(this, EventArgs.Empty);
                        });
                    }
                }
            });
            return result ?? new Mat();
        }

        public void OnRemove()
        {
            if (editorData.SelectedType == EditorType.Image)
            {
                if (imageFromFile != null)
                {
                    Funcad.ReleaseMat(imageFromFile);
                }
            }
            else if (editorData.SelectedType == EditorType.Buffer)
            {
                if (editorData.BufferImage != null)
                {
                    Funcad.ReleaseMat(editorData.BufferImage);
                }
            }
            else if (editorData.SelectedType == EditorType.FromEditor)
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

        public object GetSaveableObject()
        {
            return new HandlerSaveableClass()
            {
                Name = this.GetType().Name.Substring(0, this.GetType().Name.Length - 9),
                Params = "",
            };
        }

        public void SetSaveableObject(object obj)
        {
            
        }
    }
}
