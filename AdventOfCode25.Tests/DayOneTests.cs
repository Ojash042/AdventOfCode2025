namespace AdventOfCode25.Tests;

using FirstDay;
public class DayOneTests
{
    private DayOne _dayOne;

    private const int StartingPosition = 50;

    [SetUp]
    public void Setup()
    {
        _dayOne = new DayOne();
        _dayOne.SetPart(Part.Two);
    }

    [Test]
    public void Test1()
    {
        int currentPosition = StartingPosition;
        currentPosition = _dayOne.GetNewDialPointerValue("R49", currentPosition);
        _ = _dayOne.GetNewDialPointerValue("L98", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(0));
    }

    [Test]
    public void Test2()
    {
        int currentPosition = StartingPosition;
        currentPosition = _dayOne.GetNewDialPointerValue("R49", currentPosition);
        _ = _dayOne.GetNewDialPointerValue("R1", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(1));
    }

    [Test]
    public void Test3()
    {
        int currentPosition = StartingPosition;
        currentPosition = _dayOne.GetNewDialPointerValue("R49", currentPosition);
        currentPosition = _dayOne.GetNewDialPointerValue("R1", currentPosition);
        _ = _dayOne.GetNewDialPointerValue("R1", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(1));
    }
    
    [Test]
    public void Test4()
    {
        int currentPosition = StartingPosition;
        currentPosition = _dayOne.GetNewDialPointerValue("R49", currentPosition);
        currentPosition = _dayOne.GetNewDialPointerValue("R1", currentPosition);
        _ = _dayOne.GetNewDialPointerValue("L1", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(1));
    }

    [Test]
    public void Test5()
    {
        int currentPosition = StartingPosition;
        currentPosition = _dayOne.GetNewDialPointerValue("L50", currentPosition);
        _ = _dayOne.GetNewDialPointerValue("L100", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(2));
    }
    
    [Test]
    public void Test6()
    {
        int currentPosition = StartingPosition;
        currentPosition = _dayOne.GetNewDialPointerValue("R50", currentPosition);
        _ = _dayOne.GetNewDialPointerValue("R100", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(2));
    }
    
    [Test]
    public void Test7()
    {
        int currentPosition = StartingPosition;
        currentPosition = _dayOne.GetNewDialPointerValue("L50", currentPosition);
        _ = _dayOne.GetNewDialPointerValue("L400", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(5));
    }
    
    [Test]
    public void Test8()
    {
        int currentPosition = StartingPosition;
        currentPosition = _dayOne.GetNewDialPointerValue("L50", currentPosition);
        _ = _dayOne.GetNewDialPointerValue("R400", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(5));
    }

    [Test]
    public void Test9()
    {
        int currentPosition = StartingPosition;
        _ = _dayOne.GetNewDialPointerValue("R1000", currentPosition);
        Assert.That(_dayOne.GetResults(), Is.EqualTo(10));
    }
}