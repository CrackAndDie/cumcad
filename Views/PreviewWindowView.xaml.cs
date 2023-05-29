using System;
using System.Windows;
using System.Windows.Interop;

namespace cumcad.Views
{
    public partial class PreviewWindowView : Window
    {
        // MVVM v rot vieban
        public PreviewWindowView()
        {
            InitializeComponent();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
                var hwndTarget = hwndSource.CompositionTarget;
                hwndTarget.RenderMode = RenderMode.SoftwareOnly;
            }
            catch 
            {
                
            }
        }
    }
}
