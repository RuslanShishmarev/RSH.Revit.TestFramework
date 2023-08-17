using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System;

using RSH.Revit.TestFramework.Services;
using RSH.Revit.TestFramework.ViewModels;
using RSH.Revit.TestFramework.Views;

namespace RSH.Revit.TestFramework.Commands
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class StartTestsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RunTestsHandler runTestsHandler = new RunTestsHandler();
            RunTestHandler runTestHandler = new RunTestHandler();

            runTestsHandler.Initialize();
            runTestHandler.Initialize();

            TestBrowserViewModel testBrowserViewModel = new TestBrowserViewModel(commandData);
            testBrowserViewModel.LoadTestAssemblyCommand.Execute(null);
            TestBrowserWnd wnd = new TestBrowserWnd();
            wnd.DataContext = testBrowserViewModel;
            
            try
            {
                ViewService.ShowWindow(wnd, testBrowserViewModel.IsModal);
            }

            catch(Exception ex)
            {
                ViewService.ShowMessage(ex.Message);
                wnd.Close();
            }

            return Result.Succeeded;
        }
    }
}
