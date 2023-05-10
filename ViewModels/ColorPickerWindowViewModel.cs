using cumcad.Models.Other;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cumcad.ViewModels
{
    internal class ColorPickerWindowViewModel : BindableBase
    {
        private SolidColorBrush borderColor;
        public SolidColorBrush BorderColor
        {
            get { return borderColor; }
            set { SetProperty(ref borderColor, value); }
        }

        private BitmapImage pickerImage;
        public BitmapImage PickerImage
        {
            get { return pickerImage; }
            set { SetProperty(ref pickerImage, value); }
        }

        private bool? isSelected;

        #region Commands
        public ICommand MouseUpCommand { get; set; }
        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand DoneCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        #endregion

        internal ColorPickerWindowViewModel()
        {
            MouseUpCommand = new DelegateCommand(OnMouseUpCommand);
            MouseDownCommand = new DelegateCommand(OnMouseDownCommand);
            MouseMoveCommand = new DelegateCommand(OnMouseMoveCommand);
            DoneCommand = new DelegateCommand(OnDoneCommand);
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);

            PickerImage = new BitmapImage(new Uri("pack://application:,,,/Resources/color_palette.png"));

            // setting default color
            BorderColor = new SolidColorBrush(Colors.AliceBlue);
        }

        private bool mouseDowned = false;

        private void OnMouseUpCommand(object parameter)
        {
            var p = parameter as MouseEventArgs;
            var pos = p.GetPosition(p.Source as IInputElement);
            mouseDowned = false;
            BorderColor = new SolidColorBrush(GetPixelColor(PickerImage, (int)pos.X, (int)pos.Y));
        }

        private void OnMouseDownCommand(object parameter)
        {
            var p = parameter as MouseEventArgs;
            var pos = p.GetPosition(p.Source as IInputElement);
            mouseDowned = true;
            BorderColor = new SolidColorBrush(GetPixelColor(PickerImage, (int)pos.X, (int)pos.Y));
        }

        private void OnMouseMoveCommand(object parameter)
        {
            if (mouseDowned)
            {
                var p = parameter as MouseEventArgs;
                var pos = p.GetPosition(p.Source as IInputElement);
                BorderColor = new SolidColorBrush(GetPixelColor(PickerImage, (int)pos.X, (int)pos.Y));
            }
        }

        private static Color GetPixelColor(BitmapSource bitmap, int x, int y)
        {
            if (bitmap.PixelWidth > x && x >=0 && bitmap.PixelHeight > y && y >= 0)
            {
                CroppedBitmap croppedBitmap = new CroppedBitmap(bitmap, new Int32Rect(x, y, 1, 1));
                byte[] pixel = new byte[bitmap.Format.BitsPerPixel / 8];
                croppedBitmap.CopyPixels(pixel, bitmap.Format.BitsPerPixel / 8, 0);
                return Color.FromRgb(pixel[2], pixel[1], pixel[0]);
            }
            return Colors.AliceBlue;
        }

        async internal Task<SolidColorBrush> GetSelectionTask()
        {
            await Task.Run(() =>
            {
                while (isSelected == null)
                {

                }
            });
            return (bool)isSelected ? BorderColor : null;
        }

        private void OnDoneCommand(object paramenter)
        {
            isSelected = true;
            (paramenter as Window).Close();
        }

        private void OnCloseWindowCommand(object paramenter)
        {
            isSelected = false;
            (paramenter as Window).Close();
        }
    }
}
