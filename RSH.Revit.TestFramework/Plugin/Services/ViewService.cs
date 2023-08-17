using System.Windows;

using RSH.Revit.TestFramework.Views;

namespace RSH.Revit.TestFramework.Services
{
    internal class ViewService
    {
        public static Window ShowWindow(Window window, bool isDialog = true)
        {
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if(isDialog) window.ShowDialog();
            else window.Show();
            return window;
        }
        public static Window ShowMessage(string text)
        {
            CommonMessageWnd messageWnd = new CommonMessageWnd(text);

            return ShowWindow(messageWnd);
        }
    }
}
