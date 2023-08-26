# RSH.Revit.TestFramework
## Desctription
### Open Source test framework for Revit plugins development.
1) If you want to create tests methods you should create class in RSH.Revit.TestFramework.Tests namespace with TestRevitClassAttribute. There is folder with name "Tests".
2) To interact with the console view test class should be inherited by TestRevitBase.
3) Example test methods:
#### Simple test method without transaction with one argument
```
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
```
#### Simple test method with transaction. A new transaction will be created with the spectial name or method name when the test is run
```
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
```
### View and result
![TestFramework 1](https://github.com/RuslanShishmarev/RSH.Revit.TestFramework/assets/50487026/6ff84250-7ccf-4a34-9790-c2913470b795)
![TestFramework 2](https://github.com/RuslanShishmarev/RSH.Revit.TestFramework/assets/50487026/a4061259-5b50-4109-8201-9cd9f986dca1)



