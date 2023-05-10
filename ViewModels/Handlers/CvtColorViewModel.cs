using cumcad.Models;
using cumcad.Models.Factories;
using cumcad.ViewModels.Base;
using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.ViewModels.Handlers
{
    internal class CvtColorViewModel : BindableBase, IHandler
    {
        public EditorPageModel HandlerEditorModel { get; set; }

        private List<ColorConversionCodes> typeItems;
        public List<ColorConversionCodes> TypeItems
        {
            get { return typeItems; }
            set { SetProperty(ref typeItems, value); }
        }

        private ColorConversionCodes selectedType;
        public ColorConversionCodes SelectedType
        {
            get { return selectedType; }
            set { SetProperty(ref selectedType, value); PropertiesChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler<EventArgs> PropertiesChanged;

        public CvtColorViewModel()
        {
            TypeItems = Enum.GetValues(typeof(ColorConversionCodes)).Cast<ColorConversionCodes>().OrderBy(x => x.ToString()).ToList();
        }

        public async Task<List<Mat>> GetResult(List<Mat> images)
        {
            var mats = new List<Mat>();
            await Task.Run(() =>
            {
                foreach (var image in images)
                {
                    try
                    {
                        mats.Add(image.CvtColor(SelectedType));
                    }
                    catch (Exception ex)
                    {
                        MessageBoxFactory.Show("Something went wrong, check out the next message", MessageBoxFactory.WARN_LOGO);
                        MessageBoxFactory.Show(ex.Message, MessageBoxFactory.WARN_LOGO);
                    }
                }
            });
            return mats;
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
    }
}
