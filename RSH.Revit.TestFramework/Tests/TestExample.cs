using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RSH.Revit.TestFramework.Api;

namespace RSH.Revit.TestFramework
{
    [TestRevitClass]
    internal class TestExample : TestRevitBase
    {
        private ExternalCommandData _commandData;
        private Document _doc;

        public TestExample(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = commandData.Application.ActiveUIDocument.Document;
        }

        [TestRevitMethod(0)]
        public void OuterWallTypeTest(int testArgument)
        {
            WriteLine(nameof(OuterWallTypeTest));

            WriteLine(testArgument.ToString());
            WriteLine("--------------------------------");

            Asset.IsEqual(testArgument, 0);
        }        
    }
}
