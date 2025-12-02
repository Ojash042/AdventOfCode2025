using AdventOfCode25.SecondDay;

namespace AdventOfCode25;

internal static class Program
{
    private static void Main()
    {
        var solution = new DayTwo();
        solution.SetInputData(DataType.RealData);
        solution.Solve(Part.Two);
        Console.WriteLine(solution.GetResults());
    }
}