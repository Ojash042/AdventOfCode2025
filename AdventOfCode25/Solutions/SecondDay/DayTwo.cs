using Microsoft.Extensions.Configuration;

namespace AdventOfCode25.SecondDay;

internal record DayTwoInput(string Ranges);

public class DayTwo : ISolutions<Int128>
{
    private DayTwoInput? _inputData;
    private readonly List<Int128> _invalidNumbers = [];
    private Part _part;
    
    public Int128 GetResults()
    {
        return _invalidNumbers.Sum(num => (long) num);
    }

    public void SetPart(Part part)
    {
        _part = part;
    }

    public void SetInputData(DataType dataType)
    {
        string rangesJson;
        
        string savePath = Path.Combine(AppContext.BaseDirectory, "Inputs");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(savePath)
            .AddJsonFile("DayTwoInputs.json", optional:false, reloadOnChange:true)
            .Build();

        switch (dataType)
        { 
            default:
            case DataType.TestData:
                rangesJson = configuration["testData"] ?? string.Empty; 
                break;
                
            case DataType.RealData:
                rangesJson = configuration["realData"] ?? string.Empty;
                break;
        }

        _inputData = new DayTwoInput(rangesJson);
    }
    

    public static bool CheckSubsetDigitRepeatsTwice(Int128 number)
    {
        string numberString = number.ToString();
        
        if (numberString.Length % 2 != 0)
            return false;
        
        int partitionPoint = (int)Math.Floor((double)numberString.Length / 2);
        string firstHalf = numberString[..partitionPoint];
        string secondHalf = numberString[(partitionPoint)..];

        return firstHalf == secondHalf;
    }

    public static bool CheckSubsetDigitsRepeatsMultipleTimes(Int128 number)
    {
        string numberString = number.ToString();
        
        int partitionPoint = (int)Math.Floor((double)numberString.Length / 2);

        for (int i = 0; i < partitionPoint; i++)
        {
            string subString = numberString[..(i+1)];
            
            if (numberString.Length % subString.Length != 0) continue;
            
            int multiples = numberString.Length / subString.Length;
            string matchingString = string.Concat(Enumerable.Repeat(subString, multiples));

            if (matchingString == numberString)
                return true;
        }

        return false;
    }

    private void SumUpInvalidValuesInRange(List<Int128> rangeList)
    {
        
        if (rangeList.Count == 1)
        {
            var element = rangeList.First();
            bool repeatingCheck = _part == Part.One
                ? CheckSubsetDigitRepeatsTwice(element)
                : CheckSubsetDigitsRepeatsMultipleTimes(element);
            
            if(repeatingCheck)
                _invalidNumbers.Add(element);

            return;
        }

        int partitionPosition = (int)Math.Floor((double) rangeList.Count / 2);
        var subRangeList = rangeList
            .Take(partitionPosition)
            .ToList();
        
        SumUpInvalidValuesInRange(subRangeList);
        
        subRangeList = rangeList
            .Skip(partitionPosition)
            .Take(rangeList.Count - partitionPosition)
            .ToList();
        
        SumUpInvalidValuesInRange(subRangeList);
    }

    
    private void ParseValues()
    {
        string[] ranges = _inputData!.Ranges.Split(",");
        foreach (string range in ranges)
        {

            var min = Int128.Parse(range.Split("-").First());
            
            var max = Int128.Parse(range.Split("-").Last());
            
            var enumerable = Services.EnumerateLargeIntegerRange(min, max)
                .ToList();
            
            SumUpInvalidValuesInRange(enumerable);
        } 
    }

    public void Solve(Part part = Part.One)
    {
        SetPart(part);
        
        if (_inputData == null) SetInputData(DataType.TestData);
        ParseValues();
    }
}