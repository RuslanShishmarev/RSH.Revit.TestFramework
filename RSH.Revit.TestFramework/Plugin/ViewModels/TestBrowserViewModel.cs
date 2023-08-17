using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using RSH.Revit.TestFramework.Api;
using RSH.Revit.TestFramework.Api.Interfaces;
using RSH.Revit.TestFramework.Commands;
using RSH.Revit.TestFramework.Models;
using RSH.Revit.TestFramework.Services;

namespace RSH.Revit.TestFramework.ViewModels
{
    internal class TestBrowserViewModel : ViewModelBase, IConsoleTestBrowser
    {
        #region PROPS
        private string _testAssemlyPath;
        public string TestAssemblyPath
        {
            get => _testAssemlyPath;
            private set 
            { 
                _testAssemlyPath = value;
                OnPropertyChanged(nameof(TestAssemblyPath));
            }
        }

        private ObservableCollection<TestCaseView> _testCaseViews = new ObservableCollection<TestCaseView>();
        public ObservableCollection<TestCaseView> TestCaseViews
        {
            get => _testCaseViews;
            private set 
            {
                _testCaseViews = value;
                OnPropertyChanged(nameof(TestCaseViews));
            }
        }

        private TestCaseView _selectedTestCaseView;
        public TestCaseView SelectedTestCaseView
        {
            get => _selectedTestCaseView;
            set
            {
                _selectedTestCaseView = value;
                OnPropertyChanged(nameof(SelectedTestCaseView));
            }
        }

        private string _consoleContent = string.Empty;
        public string ConsoleContent
        {
            get => _consoleContent;
            set 
            { 
                _consoleContent = value;
                OnPropertyChanged(nameof(ConsoleContent));
            }
        }

        public bool IsModal { get; } = false;

        #endregion

        #region COMMANDS

        public RelayCommand LoadTestAssemblyCommand { get; private set; }

        public RelayCommand RunTestMethodsCommand { get; private set; }

        public RelayCommand ShowStackTraceCommand { get; private set; }

        public RelayCommand ClearConsoleContentCommand { get; private set; }

        public RelayCommand CopyElementIdCommand { get; private set; }

        public RelayCommand RunSelectedTestCommand { get; private set; }

        #endregion

        private ExternalCommandData _commandData;
        private UIApplication _uiapp;
        private Document _doc;
        private RunTestsHandler _runTestsHandler;
        private RunTestHandler _runTestHandler;

        public TestBrowserViewModel(ExternalCommandData commandData, RunTestsHandler runTestsHandler = null, RunTestHandler runTestHandler = null)
        {
            _commandData = commandData;
            _uiapp = _commandData.Application;
            _doc = _uiapp.ActiveUIDocument.Document;
            _runTestsHandler = runTestsHandler;
            _runTestHandler = runTestHandler;

            if (_runTestHandler is null || runTestHandler is null)
            {
                IsModal = true;
            }

            LoadTestAssembly();

            LoadTestAssemblyCommand = new RelayCommand(LoadTestAssembly);
            RunTestMethodsCommand = new RelayCommand(RunTestMethods);
            ShowStackTraceCommand = new RelayCommand(ShowStackTrace);
            ClearConsoleContentCommand = new RelayCommand(ClearConsoleContent);
            CopyElementIdCommand = new RelayCommand(CopyElementId);
            RunSelectedTestCommand = new RelayCommand(RunSelectedTest);
        }

        #region METHODS
        private void LoadTestAssembly()
        {
            TestCaseViews.Clear();
            ClearConsoleContent();

            try
            {
                Assembly testAssembly = Assembly.GetExecutingAssembly();

                // get all classes for test browser
                var testClassesTypes = testAssembly.GetTypes();
                var testClasses = testClassesTypes.Where(c => c.CustomAttributes.Any(x => x.AttributeType.Name == typeof(TestRevitClassAttribute).Name));

                if (testClasses.Any())
                {
                    foreach (Type testClassesType in testClasses)
                    {
                        // create instances
                        var testClassesInstance = Activator.CreateInstance(testClassesType, new object[] { _commandData });

                        if (testClassesInstance is TestRevitBase testClassesInstanceAsTestRevitBase) testClassesInstanceAsTestRevitBase.Setup(this);

                        var testMethods = testClassesType.GetMethods().Where(IsMethodTest);

                        foreach (MethodInfo methodInfo in testMethods)
                        {
                            var attributes = methodInfo.GetCustomAttributes(typeof(TestRevitMethodAttribute)).Cast<TestRevitMethodAttribute>();
                            foreach (TestRevitMethodAttribute attribute in attributes)
                            {
                                TestCaseView testCase = null;
                                if (attribute.Argument1 != null && attribute.Argument2 is null)
                                {
                                    testCase = new TestCaseView(
                                    testClassInstance: testClassesInstance,
                                    argument: attribute.Argument1,
                                    method: methodInfo);
                                }
                                else if (attribute.Argument1 != null && attribute.Argument2 != null)
                                {
                                    testCase = new TestCaseView(
                                    testClassInstance: testClassesInstance,
                                    argument1: attribute.Argument1,
                                    argument2: attribute.Argument2,
                                    method: methodInfo);
                                }

                                TestCaseViews.Add(testCase);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ConsoleContent = ex.Message;
            }
        }

        private bool IsMethodTest(MethodInfo methodInfo)
        {
            var attributes = methodInfo.GetCustomAttributes(typeof(TestRevitMethodAttribute));

            if (methodInfo.IsPublic && attributes.Any()) return true;

            return false;
        }

        private void ClearConsoleContent()
        {
            ConsoleContent = string.Empty;
        }

        private void CopyElementId(object elementIdObj)
        {
            if (elementIdObj == null) return;

            string elementId = elementIdObj as string;
            Clipboard.SetText(elementId);

            if (elementIdObj.ToString().Contains(","))
            {
                string[] ids = elementIdObj.ToString().Split(',');
                var elementIds = ids.Select(id => new ElementId(int.Parse(id))).ToArray();
                _uiapp.ActiveUIDocument.Selection.SetElementIds(elementIds);
            }
            else
            {
                _uiapp.ActiveUIDocument.Selection.SetElementIds(new ElementId[] { new ElementId(int.Parse(elementId)) });
            }
        }

        private void RunTestMethods(object testListView)
        {
            ClearConsoleContent();

            _runTestsHandler?.Raise(TestCaseViews);

            if (_runTestsHandler is null)
            {
                using (Transaction testTr = new Transaction(_doc))
                {
                    testTr.Start("Tests");
                    foreach (var test in TestCaseViews)
                    {
                        test.Run();
                    }

                    testTr.Commit();
                }
            }

            RefreshView(testListView);
        }

        private void ShowStackTrace(object testCaseObj)
        {
            TestCaseView testCase = testCaseObj as TestCaseView;

            ViewService.ShowMessage(testCase.StackTrace);
        }

        private void RunSelectedTest(object testListView)
        {
            if (SelectedTestCaseView != null)
            {
                ClearConsoleContent();

                if (_runTestHandler != null)
                {
                    _runTestHandler.Raise(SelectedTestCaseView);
                }
                else
                {
                    using (Transaction testTr = new Transaction(_doc))
                    {
                        testTr.Start(SelectedTestCaseView.Name);
                        SelectedTestCaseView.Run();
                        testTr.Commit();
                    }
                }

                RefreshView(testListView);
            }
        }

        #endregion

        private void RefreshView(object testListView)
        {
            ListView listViewTests = testListView as ListView;
            listViewTests?.Items.Refresh();
        }

        public void SetConsoleText(string text)
        {
            ConsoleContent += "\n" + text;
        }
    }
}
