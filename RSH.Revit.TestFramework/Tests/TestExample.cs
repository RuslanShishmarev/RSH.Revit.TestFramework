using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RSH.Revit.TestFramework.API;

using System.Linq;

namespace RSH.Revit.TestFramework
{
    internal class TestExample : TestRevitBase
    {        
        //Simple test method without transaction with one argument
        [TestRevitMethod(0)]
        public void MethodWithoutTransactionTest(int testArgument)
        {
            WriteLine(nameof(MethodWithoutTransactionTest));

            WriteLine(testArgument.ToString());
            WriteLine("--------------------------------");

            Asset.IsEqual(testArgument, 0);
        }

        //Simple test method without transaction with one argument
        [TestRevitMethod(15, 50)] //will be with error
        [TestRevitMethod(50, 50)] //will be succeded
        public void MethodWithoutTransactionTest(int testArgument1, int testArgument2)
        {
            WriteLine(nameof(MethodWithoutTransactionTest));
            WriteLine($"{testArgument1} and {testArgument2}");
            WriteLine("--------------------------------");

            int sum = testArgument1 + testArgument2;

            Asset.IsEqual(sum, 100);
        }

        //Simple test method with transaction
        //A new transaction will be created with the spectial name or method name when the test is run
        //[TransactionMethod("My method")]
        [TransactionMethod]
        [TestRevitMethod]
        public void MethodWithTransactionTest()
        {
            WriteLine(nameof(MethodWithTransactionTest));

            var wallType = new FilteredElementCollector(Doc).OfClass(typeof(WallType)).FirstOrDefault();
            var firstLevel = new FilteredElementCollector(Doc).OfClass(typeof(Level)).FirstOrDefault();

            var newTestWall = Wall.Create(
                document: Doc,
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

        [TestRevitMethod(524851)]
        public void MethodToGetWallAndCheckParams(int wallId)
        {
            var currentWall = Doc.GetElement(new ElementId(wallId));

            var wallHeight = currentWall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM)?.AsValueString();

            WriteLine(wallHeight);

            Asset.IsNotEqual(wallHeight, null);
            Asset.IsEqual(wallHeight, "3400");
        }
    }
}
