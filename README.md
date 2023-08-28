# RSH.Revit.TestFramework
## Desctription
### Open Source test framework for Revit plugins development.
1) Clone or download this project and add to your solution and run framework from "Add-In Manager" tool
2) If you want to create tests methods you should create class in RSH.Revit.TestFramework.Tests namespace with inherited by TestRevitBase. There is folder with name "Tests".
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
```
#### Simple test method without transaction with one argument
```
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
![TestFramework 1](https://github.com/RuslanShishmarev/RSH.Revit.TestFramework/assets/50487026/40da3fe4-cbce-4d0b-9039-f13d56ee78e2)
![TestFramework 2](https://github.com/RuslanShishmarev/RSH.Revit.TestFramework/assets/50487026/126f66da-7c9b-4e80-8b1c-e7638d0a566d)

If your test fails due to an exception, you can see the error text using the "Show StackTrace" button



