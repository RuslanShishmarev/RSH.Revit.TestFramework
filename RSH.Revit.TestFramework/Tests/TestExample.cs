using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RSH.Revit.TestFramework.API;

using System.Linq;

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
        
        //Simple test method without transaction
        [TestRevitMethod(0)]
        public void MethodWithoutTransactionTest(int testArgument)
        {
            WriteLine(nameof(MethodWithoutTransactionTest));

            WriteLine(testArgument.ToString());
            WriteLine("--------------------------------");

            Asset.IsEqual(testArgument, 0);
        }

        //Simple test method with transaction
        //A new transaction will be created with the spectial name or method name when the test is run
        //[TransactionMethod("My method")]
        [TransactionMethod] 
        [TestRevitMethod]
        public void MethodWithTransactionTest()
        {
            WriteLine(nameof(MethodWithTransactionTest));

            var wallType = new FilteredElementCollector(_doc).OfClass(typeof(WallType)).FirstOrDefault();
            var firstLevel = new FilteredElementCollector(_doc).OfClass(typeof(Level)).FirstOrDefault();

            var newTestWall = Wall.Create(
                document: _doc,
                curve: Line.CreateBound(XYZ.Zero, new XYZ(10, 10, 0)),
                wallTypeId: wallType.Id,
                levelId: firstLevel.Id,
                height: 10,
                offset: 0,
                flip: false,
                structural: false);

            WriteLine($"New wall id: {newTestWall.Id}");
            WriteLine("--------------------------------");

            Asset.IsNotEqual(newTestWall, null);
        }
    }
}
