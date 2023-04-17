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

namespace cumcad.ViewModels
{
    enum EditorType
    {
        Image,
        CameraStream,
        Shufflecad
    }

    class SelectEditorResult
    {
        internal bool? IsSelected { get; set; }
        internal EditorType SelectedType { get; set; }
        internal string ImagePath { get; set; }
    }

    internal class SelectEditorWindowViewModel : BindableBase
    {
        private SelectEditorResult result = new SelectEditorResult();

        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set { SetProperty(ref selectedTabIndex, value); }
        }

        private string imageFilePath;
        public string ImageFilePath
        {
            get { return imageFilePath; }
            set { SetProperty(ref imageFilePath, value); }
        }

        #region Commands
        public ICommand OpenFileCommand { get; set; }
        public ICommand CreateEditorCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        #endregion

        internal SelectEditorWindowViewModel() 
        {
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);
            CreateEditorCommand = new DelegateCommand(OnCreateWindowCommand);
            OpenFileCommand = new DelegateCommand(OnOpenFileCommand);
        }

        async internal Task<SelectEditorResult> GetSelectionTask()
        {
            await Task.Run(() =>
            {
                while (result.IsSelected == null)
                {

                }
            });
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

        private void OnCreateWindowCommand(object paramenter)
        {
            result.IsSelected = true;
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
