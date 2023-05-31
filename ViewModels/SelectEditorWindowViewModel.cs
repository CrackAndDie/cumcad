﻿using cumcad.Models.Factories;
using cumcad.Models.Helpers;
using cumcad.Models;
using cumcad.Models.Other;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using cumcad.Models.Classes;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace cumcad.ViewModels
{
    public enum EditorType
    {
        Image,
        Buffer,
        CameraStream,
        Shufflecad,
        FromEditor
    }

    internal class SelectEditorWindowViewModel : BindableBase
    {
        private SelectEditorResult result = new SelectEditorResult();

        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set { SetProperty(ref selectedTabIndex, value); result.SelectedType = (EditorType)value; }
        }

        // for IMAGE
        private string imageFilePath;
        public string ImageFilePath
        {
            get { return imageFilePath; }
            set { SetProperty(ref imageFilePath, value); }
        }

        // for BUFFER
        private BitmapImage bufferImage;
        public BitmapImage BufferImage
        {
            get { return bufferImage; }
            set { SetProperty(ref bufferImage, value); }
        }

        private SolidColorBrush iconColor;
        public SolidColorBrush IconColor
        {
            get { return iconColor; }
            set { SetProperty(ref iconColor, value); result.IconColor = value; }
        }

        #region Commands
        public ICommand OpenFileCommand { get; set; }
        public ICommand PasteFromBufferCommand { get; set; }
        public ICommand OpenColorPickerCommand { get; set; }
        public ICommand CreateEditorCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        #endregion

        internal SelectEditorWindowViewModel() 
        {
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);
            CreateEditorCommand = new DelegateCommand(OnCreateWindowCommand);
            OpenFileCommand = new DelegateCommand(OnOpenFileCommand);
            PasteFromBufferCommand = new DelegateCommand(OnPasteFromBufferCommand);
            OpenColorPickerCommand = new DelegateCommand(OnOpenColorPickerCommand);

            // setting default color
            IconColor = new SolidColorBrush(Colors.AliceBlue);
        }

        async internal Task<SelectEditorResult> GetSelectionTask()
        {
            await Task.Run(() =>
            {
                while (result.IsSelected == null)
                {

                }
            });
            result.EditorIndex = Funcad.GetNewEditorIndex();
            return result;
        }

        private void OnOpenFileCommand(object paramenter)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Browse Image File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "png",
                Filter = "png files (*.png)|*.png",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if ((bool)openFileDialog.ShowDialog())
            {
                string selectedFile = openFileDialog.FileName;

                ImageFilePath = selectedFile;
            }
        }

        private void OnPasteFromBufferCommand(object parameter)
        {
            if (Clipboard.ContainsImage())
            {
                System.Windows.Forms.IDataObject clipboardData = System.Windows.Forms.Clipboard.GetDataObject();
                if (clipboardData != null)
                {
                    if (clipboardData.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap))
                    {
                        Bitmap bitmap = (Bitmap)clipboardData.GetData(System.Windows.Forms.DataFormats.Bitmap);
                        using (var memory = new MemoryStream())
                        {
                            bitmap.Save(memory, ImageFormat.Png);
                            memory.Position = 0;

                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memory;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            bitmapImage.Freeze();

                            BufferImage = bitmapImage;
                            memory.Position = 0;
                            result.BufferImage = OpenCvSharp.Mat.FromStream(memory, OpenCvSharp.ImreadModes.Unchanged);
                        }
                    }
                }
            }
        }

        async private void OnOpenColorPickerCommand(object paramenter)
        {
            var brush = await SelectorsFactory.OpenColorPickerWindow();
            if (brush != null)
            {
                IconColor = brush;
            }
        }

        private void OnCreateWindowCommand(object paramenter)
        {
            if (result.SelectedType == EditorType.Image)
                result.IsSelected = ImageFilePath != null;
            else if (result.SelectedType == EditorType.Buffer)
                result.IsSelected = result.BufferImage != null;
            result.ImagePath = ImageFilePath;
            (paramenter as Window).Close();
        }

        private void OnCloseWindowCommand(object paramenter)
        {
            result.IsSelected = false;
            (paramenter as Window).Close();
        }
    }
}
