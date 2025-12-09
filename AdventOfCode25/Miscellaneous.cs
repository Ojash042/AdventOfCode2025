using System.Data;
using Microsoft.VisualBasic.CompilerServices;

namespace AdventOfCode25;


internal record DayOneInput(string Document, int StartingPosition);
internal record DayTwoInput(string Ranges);
internal record DayThreeInput(string Banks);
internal record DayFourInput(string RollArrangement);
internal record DayFiveInput(string Ranges, string Ingredients);
internal record DaySixInput(string Arithmetic);
internal record DaySevenInput(string TachyonManifold);
internal record DayEightInput(string JunctionPosition);

public interface ISolutions<out T>
{
    public T GetResults();
    public void SetInputData(DataType dataType);
    public void Solve(Part part);
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
    
    public static IEnumerable<long> EnumerateLargeIntegerRange(long min, long max)
    {
        for (long i = min; i <= max; i++)
        {
            yield return i;
        }
    }
}

public static class Extensions
{
    public static long Product(this List<int> source)
    {
        int partitionPoint = source.Count / 2;
        return source.Count switch
        {
            1 => source.First(),
            2 => source.First() * source.Last(),
            _ => Product(source[..partitionPoint]) * Product(source[(partitionPoint )..])
        };
    }


    public static Int128 GetDistance(this Dictionary<(int, int), Int128> source, int row, int col)
    {
        int min = Math.Min(row, col);
        int max = Math.Max(row, col);
        return source[(min, max)] ;
    }
    public static Dictionary<(int, int), long> SetDistance(this Dictionary<(int, int), long> source, int row, int col, long value)
    {
        int min = Math.Min(row, col);
        int max = Math.Max(row, col);
        source[(min, max)] = value;
        return source;
    }
}