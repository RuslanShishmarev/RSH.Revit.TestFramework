using System.Windows;

namespace RSH.Revit.TestFramework.Views
{
    /// <summary>
    /// Логика взаимодействия для CommonMessageWnd.xaml
    /// </summary>
    public partial class CommonMessageWnd : Window
    {
        public CommonMessageWnd(string text)
        {
            InitializeComponent();
            textView.Text = text;
        }
    }
}
