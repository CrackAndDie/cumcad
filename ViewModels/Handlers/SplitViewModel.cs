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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cumcad.ViewModels.Handlers
{
    internal class SplitViewModel : BindableBase, IHandler, ISaveable
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private List<int> channelItems;
        public List<int> ChannelItems
        {
            get { return channelItems; }
            set { SetProperty(ref channelItems, value); }
        }

        private int selectedChannel;
        public int SelectedChannel
        {
            get { return selectedChannel; }
            set { SetProperty(ref selectedChannel, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public async Task<Mat> GetResult(Mat image)
        {
            var mat = new Mat();
            await Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GenerateList(image.Channels());
                    });

                    Cv2.Split(image, out Mat[] mats);
                    Funcad.ReleaseMat(mat);
                    mat = mats[SelectedChannel];
                    for (int i = 0; i < mats.Length; i++)
                    {
                        if (i != SelectedChannel)
                            Funcad.ReleaseMat(mats[i]);
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

        private void GenerateList(int count)
        {
            ChannelItems = Enumerable.Range(0, count).ToList();
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
                Params = string.Join(";", new int[] { SelectedChannel, ChannelItems.Count }),
            };
        }

        public void SetSaveableObject(object obj)
        {
            var hsc = obj as HandlerSaveableClass;
            string[] items = hsc.Params.Split(';');
            GenerateList(int.Parse(items[1]));
            SelectedChannel = int.Parse(items[0]);
        }
    }
}
