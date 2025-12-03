using AdventOfCode25.Solutions;

namespace AdventOfCode25;

internal static class Program
{
    private static void Main()
    {
        var solution = new DayThree();
        const DataType dataType = DataType.RealData;
        const Part part = Part.Two;
        solution.SetInputData(dataType);
        solution.Solve(part);
        
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Final Result: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(solution.GetResults());
            Console.ResetColor();
            Console.WriteLine();    
        }
        
    }
}