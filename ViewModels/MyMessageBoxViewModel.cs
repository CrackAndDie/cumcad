using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using cumcad.ViewModels.Base;
using cumcad.Models.Other;
using cumcad.Models.Factories;

namespace cumcad.ViewModels
{
    internal class MyMessageBoxViewModel : BaseViewModel
    {
        private const string resourcesPath = "pack://application:,,,/Resources/";

        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; OnPropertyChanged(); }
        }

        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set { messageText = value; OnPropertyChanged(); }
        }

        private Visibility okSPVisibility;
        public Visibility OkSPVisibility
        {
            get { return okSPVisibility; }
            set { okSPVisibility = value; OnPropertyChanged(); }
        }

        private Visibility yesNoSPVisibility;
        public Visibility YesNoSPVisibility
        {
            get { return yesNoSPVisibility; }
            set { yesNoSPVisibility = value; OnPropertyChanged(); }
        }

        private int gotResult = 0;

        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand AcceptCloseWindowCommand { get; set; }
        #endregion

        internal MyMessageBoxViewModel() : this("", MessageBoxFactory.WARN_LOGO, MessageBoxFactory.OK_TYPE) { }

        internal MyMessageBoxViewModel(string msg, string image, int buttonsType)
        {
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommand);
            AcceptCloseWindowCommand = new DelegateCommand(OnAcceptCloseWindowCommand);
            MessageText = msg;
            ImageSource = new BitmapImage(new Uri(resourcesPath + image));

            CollapseAll();
            switch (buttonsType)
            {
                case MessageBoxFactory.OK_TYPE:
                    OkSPVisibility = Visibility.Visible;
                    break;
                case MessageBoxFactory.YES_NO_TYPE:
                    YesNoSPVisibility = Visibility.Visible;
                    break;
            }
        }

        private void CollapseAll()
        {
            OkSPVisibility = Visibility.Collapsed;
            YesNoSPVisibility = Visibility.Collapsed;
        }

        async internal Task<bool> GetResult()
        {
            await Task.Run(() =>
            {
                while (gotResult == 0) ;
            });
            return gotResult == 2;
        }

        private void OnCloseWindowCommand(object paramenter)
        {
            gotResult = 1;
            (paramenter as Window).Close();
        }

        private void OnAcceptCloseWindowCommand(object paramenter)
        {
            gotResult = 2;
            (paramenter as Window).Close();
        }
    }
}
