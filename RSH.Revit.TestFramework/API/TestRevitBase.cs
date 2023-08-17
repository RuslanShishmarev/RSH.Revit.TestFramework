using RSH.Revit.TestFramework.Api.Interfaces;

namespace RSH.Revit.TestFramework.Api
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