using System.ComponentModel;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode25.Solutions;

public class DaySeven : ISolutions<long>
{
    private DaySevenInput? _inputData;
    private Part _part = Part.One;
    
    private string[] _tachyonMap = [];
    private int _splitCount;
    private readonly Dictionary<int, long> _timelineCount = new(); 
    public long GetResults()
    {
        return _part == Part.One ? _splitCount : _timelineCount.Values.Sum();
    }

    public void SetInputData(DataType dataType)
    {
        string tachyonManifold;
        
        string solutionName = TypeDescriptor
            .GetClassName(this)!
            .Split('.').Last();
        string jsonFileName = $"{solutionName}Inputs.json";
        
        string savePath = Path.Combine(AppContext.BaseDirectory, "Inputs");
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(savePath)
            .AddJsonFile(jsonFileName, optional:false, reloadOnChange:true)
            .Build();

        switch (dataType)
        { 
            default:
            case DataType.TestData:
                tachyonManifold = configuration["testData"] ?? string.Empty; 
                break;
                
            case DataType.RealData:
                tachyonManifold = configuration["realData"] ?? string.Empty;
                break;
        }

        _inputData = new DaySevenInput(tachyonManifold);
    }

    private (string, List<int>) CreateNewTachyonPoints(string currentTachyonField, int startPoint)
    {
         char newTachyonPoint =  currentTachyonField[startPoint];
         List<int> childTachyonPoints = [];
         var newTachyonField = new StringBuilder(currentTachyonField);

         if (newTachyonPoint == '^')
         {
             if (startPoint > 0)
             {
                 _timelineCount[startPoint + 1] += _timelineCount[startPoint];
                 newTachyonField[startPoint+1] = '|';
                 childTachyonPoints.Add(startPoint - 1);    
             }

             if (startPoint < currentTachyonField.Length)
             {
                 _timelineCount[startPoint - 1] += _timelineCount[startPoint];
                 newTachyonField[startPoint-1] = '|';
                 childTachyonPoints.Add(startPoint + 1);    
             }
             _timelineCount[startPoint] = 0;
             _splitCount++;
         }
         else 
         {
             newTachyonField[startPoint] = '|';
             childTachyonPoints.Add(startPoint);
         }
         
         childTachyonPoints = childTachyonPoints.Distinct().ToList();
         return (newTachyonField.ToString(), childTachyonPoints);

    }

    private void CreateTachyonBeam(string[] remainingTachyonFields, List<int> beamStartPoints)
    {
        if (remainingTachyonFields.Length == 0)
        {
            return ;
        }

        string newField = remainingTachyonFields.First();
        
        List<int> nextStartingPoint = [];
        beamStartPoints.ForEach(start =>
        { 
            (newField, List<int> childPoints)= CreateNewTachyonPoints(newField, start);
            childPoints = childPoints.Distinct().ToList();
            nextStartingPoint.AddRange(childPoints);
            nextStartingPoint =  nextStartingPoint.Distinct().ToList();
        });
        
        _tachyonMap = _tachyonMap
            .Append(newField)
            .ToArray();
        
        nextStartingPoint.Sort();
        
        CreateTachyonBeam(remainingTachyonFields[1..], nextStartingPoint);
    }
    
    
    public void Solve(Part part)
    {
        _part = part;
        if (_inputData == null)
        {
            throw new Exception("No Input Data Found");
        }

        string[] tachyonField = _inputData.TachyonManifold
            .Split('\n');
        
        int startingPoint =  tachyonField
            .First()
            .IndexOf('S');
        _tachyonMap = _tachyonMap
            .Append(tachyonField.First())
            .ToArray();

        for (int i = 0; i < tachyonField.First().Length; i++)
        {
            _timelineCount[i] = 0;   
        }
        
        _timelineCount[startingPoint] = 1;
        CreateTachyonBeam(tachyonField[1..], [startingPoint]);
    }
}