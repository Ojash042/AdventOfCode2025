using Microsoft.Extensions.Configuration;

namespace AdventOfCode25.FirstDay;

public record DayOneInput(string Document, int StartingPosition);

public class DayOne : ISolutions<int>
{
    private const int MaxDialValue = 100;
    private int _counter; 
    private Part _part;
    private DayOneInput? _inputData;
    
    public int GetResults()
    {
        return _counter;
    }
    
    public void SetPart(Part part)
    {
        _part = part;
    }
    
    public void SetInputData(DataType dataType = DataType.TestData)
    {
        string jsonDocument;
        int startingPosition;
        
        string savePath = Path.Combine(AppContext.BaseDirectory, "Inputs");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(savePath)
            .AddJsonFile("DayOneInputs.json", optional:false, reloadOnChange:true)
            .Build();

        switch (dataType)
        { 
            default:
            case DataType.TestData:
                startingPosition = int.Parse(configuration["testData:startingPoint"] ?? "0");
                jsonDocument = configuration["testData:directions"] ?? string.Empty; 
                break;
                
            case DataType.RealData:
                startingPosition = int.Parse(configuration["realData:startingPoint"] ?? "0");
                jsonDocument = configuration["realData:directions"] ?? string.Empty;
                break;
        }

        _inputData = new DayOneInput(jsonDocument, startingPosition);
    }
    
    public int GetNewDialPointerValue(string command, int currentPosition)
    {
        char direction = command[0];
        int steps = int.Parse(command[1..]);
        int newPosition = direction switch
        {
            'L' => (currentPosition - steps),
            'R' => (currentPosition + steps),
            _ => currentPosition
        };
        
        if (_part == Part.One)
        {
            newPosition = ((newPosition % MaxDialValue) + MaxDialValue) % MaxDialValue;
            _counter += newPosition == 0 ? 1 : 0;
            return newPosition;
        }

        double turns = Math.Abs((double)newPosition / MaxDialValue);
        int multiple = (int) ( Math.Floor(turns));
        _counter += multiple;
        _counter += newPosition <= 0 ? 1 : 0; 
        _counter += (currentPosition == 0 && newPosition < 0) ? -1 : 0;
        
        newPosition = ((newPosition % MaxDialValue) + MaxDialValue) % MaxDialValue;
        return newPosition;
    }
    
    private void RetrievePassword()
    {
        int newPosition = _inputData!.StartingPosition ;
        string[] lines = _inputData.Document.Split('\n');
        _ = lines.Aggregate(newPosition, (current, line) => GetNewDialPointerValue(line, current));
    }
    
    public void Solve(Part part = Part.One)
    {
        _part = part;
        if (_inputData is null){ SetInputData();}
        RetrievePassword();
    }
}
