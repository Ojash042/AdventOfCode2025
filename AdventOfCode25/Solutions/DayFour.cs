using System.Text;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode25.Solutions;

public class DayFour : ISolutions<int>
{
    private DayFourInput? _inputData;
    private Part _part;
    private int _validRolls;
    
    public int GetResults()
    {
        return _validRolls;
    }

    public void SetInputData(DataType dataType)
    {
        string banksJson;
        
        string savePath = Path.Combine(AppContext.BaseDirectory, "Inputs");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(savePath)
            .AddJsonFile("DayFourInputs.json", optional:false, reloadOnChange:true)
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

        _inputData = new DayFourInput(banksJson);
    }
    
    private static char GetNthCharacter(string paperRoll, int n)
    {
        if (n < 0 || n >= paperRoll.Length)
            return '/';
        char roll = paperRoll.ElementAt(n);
        return roll == '\n' ? '/' : paperRoll.ElementAt(n);
    }

    private (string, bool) TraversePaperRollArrangement(string arrangement)
    {
        int columnCount = arrangement.IndexOf('\n');
        int instanceValidRolls = 0;
        StringBuilder stringBuilder = new (arrangement);
        
        for (int i = 0; i < arrangement.Length; i++)
        {
            if (arrangement.ElementAt(i) == '\n' || arrangement.ElementAt(i) == '.') continue;
            char[] neighboursChar =
            [
                GetNthCharacter(arrangement, i - columnCount - 2),
                GetNthCharacter(arrangement, i - columnCount - 1),
                GetNthCharacter(arrangement, i - columnCount),
                GetNthCharacter(arrangement, i - 1),
                GetNthCharacter(arrangement, i + 1),
                GetNthCharacter(arrangement, i + columnCount),
                GetNthCharacter(arrangement, i + columnCount + 1),
                GetNthCharacter(arrangement, i + columnCount + 2)
            ];
            string neighbours = new (neighboursChar);

            if (neighbours.Count(paperRoll => paperRoll == '@') >= 4) continue;
            stringBuilder[i] = '.';
            instanceValidRolls++;
        }

        _validRolls += instanceValidRolls;
        return (stringBuilder.ToString(), instanceValidRolls == 0);
    }
    
    
    private void ParseInput()
    {
        string rollArrangement = _inputData!.RollArrangement;
        
        if (_part == Part.One)
            TraversePaperRollArrangement(rollArrangement);
       
        bool shouldStop = false;
        while (!shouldStop)
        {
            (rollArrangement, shouldStop) = TraversePaperRollArrangement(rollArrangement);
        }
    }

    public void Solve(Part part)
    {
        _part = part;
        if (_inputData == null) SetInputData(DataType.TestData);
        ParseInput();
    }
}