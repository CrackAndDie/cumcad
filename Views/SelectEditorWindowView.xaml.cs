using System;
using System.Windows;
using System.Windows.Data;

namespace cumcad.Views
{
    public class EditorTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((int)value).ToString() == (string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? parameter : null;
        }
    }

    public partial class SelectEditorWindowView : Window
    {
        public SelectEditorWindowView()
        {
            InitializeComponent();
        }
    }
}
