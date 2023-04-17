using cumcad.Models.Other;
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

    internal class SelectEditorWindowViewModel
    {
        private SelectEditorResult result = new SelectEditorResult();

        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        #endregion

        internal SelectEditorWindowViewModel() 
        {
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);
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

        private void OnCloseWindowCommand(object paramenter)
        {
            result.IsSelected = false;
            (paramenter as Window).Close();
        }
    }
}
