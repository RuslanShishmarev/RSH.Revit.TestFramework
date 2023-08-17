using System.Windows;

namespace RSH.Revit.TestFramework.Views
{
    /// <summary>
    /// Логика взаимодействия для TestBrowserWnd.xaml
    /// </summary>
    public partial class TestBrowserWnd : Window
    {
        public TestBrowserWnd()
        {
            InitializeComponent();
            NameScope.SetNameScope(contextMenu, NameScope.GetNameScope(this));
        }
    }
}
