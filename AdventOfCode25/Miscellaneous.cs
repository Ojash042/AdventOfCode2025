namespace AdventOfCode25;


internal record DayOneInput(string Document, int StartingPosition);
internal record DayTwoInput(string Ranges);
internal record DayThreeInput(string Banks);


public interface ISolutions<out T>
{
    public T GetResults();
    public void SetInputData(DataType dataType);
    public void Solve(Part part = Part.One);
}

public enum Part
{
    One,
    Two
}


public enum DataType 
{
    TestData,
    RealData
}

public static class Services
{
    public static IEnumerable<Int128> EnumerateLargeIntegerRange(Int128 min, Int128 max)
    {
        for (var i = min; i <= max; i++)
        {
            yield return i;
        }
    }
}