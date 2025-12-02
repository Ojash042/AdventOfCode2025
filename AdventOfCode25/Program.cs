namespace AdventOfCode25;

using FirstDay;
internal static class Program
{
    private static void Main()
    {
        var dayOnePartOne = new DayOne();
        dayOnePartOne.SetInputData(DataType.RealData);
        dayOnePartOne.Solve(Part.Two);
    }
}