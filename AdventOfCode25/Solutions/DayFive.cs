using Microsoft.Extensions.Configuration;

namespace AdventOfCode25.Solutions;

public class DayFive : ISolutions<long>
{
    private DayFiveInput? _inputData;
    private long _freshIngredients;
    private static readonly List<string> NormalizedRanges = [];
    
    public long GetResults()
    {
        return _freshIngredients;
    }

    public void SetInputData(DataType dataType)
    {
        string ingredientsRequirements;
        
        string savePath = Path.Combine(AppContext.BaseDirectory, "Inputs");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(savePath)
            .AddJsonFile("DayFiveInputs.json", optional:false, reloadOnChange:true)
            .Build();

        switch (dataType)
        { 
            default:
            case DataType.TestData:
                ingredientsRequirements = configuration["testData"] ?? string.Empty; 
                break;
                
            case DataType.RealData:
                ingredientsRequirements = configuration["realData"] ?? string.Empty;
                break;
        }

        string ranges = ingredientsRequirements.Split("\n\n").First();
        string ingredients = ingredientsRequirements.Split("\n\n").Last();
        _inputData = new DayFiveInput(ranges, ingredients);   
    }

    private bool IsNumberInRanges(long number)
    {
        foreach (string range in _inputData!.Ranges.Split('\n'))
        {
            string[] rangeValues = range.Split('-');
            long min = long.Parse(rangeValues.First());
            long max = long.Parse(rangeValues.Last());
            if (number >=min && number <=max)
            {
                return true;
            }
        }

        return false;
    }

    private void FilterFreshIngredients()
    {
        _freshIngredients = _inputData!.Ingredients
            .Split('\n')
            .Select(long.Parse)
            .Count(IsNumberInRanges);
    }

    private static void NormalizeRanges(string range)
    {
        if (NormalizedRanges.Count == 0)
        {
            NormalizedRanges.Add(range);
        }

        string lastElement = NormalizedRanges.Last();
        long minOfLastElement =  long.Parse(lastElement.Split("-").First());
        long maxOfLastElement = long.Parse(lastElement.Split("-").Last());
        long minOfNewElement = long.Parse(range.Split("-").First());
        long maxOfNewElement = long.Parse(range.Split("-").Last());

        if (minOfNewElement > maxOfLastElement)
        {
            NormalizedRanges.Add(range);
            return;
        }

        long newMaxForRange = Math.Max(maxOfNewElement, maxOfLastElement);
        NormalizedRanges[^1] = $"{minOfLastElement}-{newMaxForRange}";
    }
    
    private void CountFreshIngredients()
    {
        string[] rangesList = _inputData!.Ranges.Split("\n");
        
        rangesList.Sort((a, b) =>
            {
                long minOfA = long.Parse(a.Split("-").First());
                long minOfB = long.Parse(b.Split("-").First());
                return minOfA.CompareTo(minOfB);
            });

        foreach (string range in rangesList)
        {
            NormalizeRanges(range);
        }

        foreach (string range  in NormalizedRanges)
        {
            long min = long.Parse(range.Split('-').First());
            long max = long.Parse(range.Split('-').Last());
            _freshIngredients += max - min + 1;
        }
    }
    public void Solve(Part part)
    {
        if (_inputData == null) SetInputData(DataType.TestData);
        switch (part)
        {
            default:
            case Part.One:
                FilterFreshIngredients();
                break;
            case Part.Two:
                CountFreshIngredients();
                break;
        }
    }
}