using AdventOfCode25.SecondDay;

namespace AdventOfCode25.Tests;

[TestFixture]
public class RepeatingNumberTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(DayTwo.CheckSubsetDigitRepeatsTwice(123123), Is.True);
            Assert.That(DayTwo.CheckSubsetDigitRepeatsTwice(101), Is.False);
            Assert.That(DayTwo.CheckSubsetDigitRepeatsTwice(123124), Is.False);
        }

    }

    [Test]
    public void Test2()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(DayTwo.CheckSubsetDigitsRepeatsMultipleTimes(123123123), Is.True);
            Assert.That(DayTwo.CheckSubsetDigitsRepeatsMultipleTimes(12), Is.False);
            Assert.That(DayTwo.CheckSubsetDigitsRepeatsMultipleTimes(101), Is.False);
        }
    }
}