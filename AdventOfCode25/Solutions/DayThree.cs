using Microsoft.Extensions.Configuration;

namespace AdventOfCode25.Solutions;


public class DayThree : ISolutions<long>
{
    private Part _part;
    private long _sumOfJolts;
    private DayThreeInput? _inputData;
    private delegate long SolutionDelegate(string bank);
    
    public long GetResults()
    {
        return _sumOfJolts;
    }

    public void SetInputData(DataType dataType)
    {
        string banksJson;
        
        string savePath = Path.Combine(AppContext.BaseDirectory, "Inputs");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(savePath)
            .AddJsonFile("DayThreeInputs.json", optional:false, reloadOnChange:true)
            .Build();

        switch (dataType)
        { 
            default:
            case DataType.TestData:
                banksJson = configuration["testData"] ?? string.Empty; 
                break;
                
            case DataType.RealData:
                banksJson = configuration["realData"] ?? string.Empty;
                break;
        }

        _inputData = new DayThreeInput(banksJson);
    }

    private static SortedList<int, int> MapJoltageFirstIndex(string bank)
    {
        SortedList<int, int> joltageMap = new ();
        
        for (int index = 0; index < bank.Length; index++)
        {
            int joltValue = int.Parse(bank[index].ToString());
            joltageMap.TryAdd(joltValue, index);
        }

        return joltageMap;
    } 

    public static long GetLargestJoltage(string bank)
    {

        SortedList<int, int> joltageMap = MapJoltageFirstIndex(bank);
        
        KeyValuePair<int, int> tenthPlaceDigit = joltageMap.Last();

        if (tenthPlaceDigit.Value == bank.Length - 1)
        {
            joltageMap.Remove(tenthPlaceDigit.Key);
            tenthPlaceDigit = joltageMap.Skip(1).Last();
        }

        string substringBank = bank[(tenthPlaceDigit.Value + 1)..];

        int onesPlace = substringBank
            .Select(num => int.Parse(num.ToString()))
            .Max();
        
        int maxJoltage = tenthPlaceDigit.Key * 10 + onesPlace;
        
        return maxJoltage;
    }
    
    public static long GetNLargestJoltageBatteries(string bank, int currentIteration)
    {
        SortedList<int, int> joltageMap = MapJoltageFirstIndex(bank);

        KeyValuePair<int, int> largestDigit = new();
        string substringBank = string.Empty;
        if (currentIteration <= 0) return 0;
        
        foreach (KeyValuePair<int, int> joltIndexPair in joltageMap.Reverse())
        {
            substringBank = bank[(joltIndexPair.Value + 1)..];
            if (substringBank.Length < currentIteration-1) continue;
            largestDigit = joltIndexPair;
            break;
        }

        long mostSignificantDigit = (long)(largestDigit.Key * Math.Pow(10, currentIteration-1));
        return  mostSignificantDigit + GetNLargestJoltageBatteries(substringBank, currentIteration-1);
    }


    public static long GetNLargestJoltageBatteriesWrapper(string bank)
    {
        return GetNLargestJoltageBatteries(bank, 12);
    }

    private void ParseInput()
    {
        SolutionDelegate  solution = _part == Part.One ? GetLargestJoltage : GetNLargestJoltageBatteriesWrapper;
        string[] banks = _inputData!.Banks.Split('\n');
        
        foreach (string bank in banks)
        { 
            long value = solution(bank); 
            _sumOfJolts += value;
        }
    } 
    
    public void Solve(Part part = Part.One)
    {
        if (_inputData == null) SetInputData(DataType.TestData);
        _part = part;
        ParseInput();
    }
}