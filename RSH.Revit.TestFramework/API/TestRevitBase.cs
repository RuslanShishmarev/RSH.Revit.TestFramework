using RSH.Revit.TestFramework.API.Interfaces;

namespace RSH.Revit.TestFramework.API
{
    internal class TestRevitBase
    {
        private IConsoleTestBrowser _console;
        public void Setup(IConsoleTestBrowser console)
        {
            _console = console;
        }

        protected void WriteLine(string text)
        {
            _console.SetConsoleText(text);
        }
    }
}