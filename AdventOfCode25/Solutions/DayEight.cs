using System.ComponentModel;
using System.Numerics;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode25.Solutions;

public class DayEight : ISolutions<long>
{
    private List<List<int>> _circuits = [];
    private DayEightInput? _inputData;
    private DataType _dataType;
    private Part _part;
    private long _xCoordinateProduct;
    private Dictionary<int, string> _coordinates = new();
    private Dictionary<(int, int), long> _distances = new();
    public long GetResults()
    {
        long partOneResult = 0;
        if (_part == Part.One) 
            partOneResult  =  _circuits
            .OrderByDescending(c => c.Count)
            .Take(3).Select(c => c.Count)
            .ToList()
            .Product();
        
        return _part == Part.One ? partOneResult : _xCoordinateProduct;
    }

    public void SetInputData(DataType dataType)
    {
        string junctionPosition;
        
        string solutionName = TypeDescriptor
            .GetClassName(this)!
            .Split('.').Last();
        string jsonFileName = $"{solutionName}Inputs.json";
        
        string savePath = Path.Combine(AppContext.BaseDirectory, "Inputs");
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(savePath)
            .AddJsonFile(jsonFileName, optional:false, reloadOnChange:true)
            .Build();

        _dataType = dataType; 
        
        switch (dataType)
        { 
            default:
            case DataType.TestData:
                junctionPosition = configuration["testData"] ?? string.Empty; 
                break;
                
            case DataType.RealData:
                junctionPosition = configuration["realData"] ?? string.Empty;
                break;
        }

        _inputData = new DayEightInput(junctionPosition);
    }

    private Dictionary<(int, int), long> CalculateDistances()
    { 
        List<string> junctionPosition =  _inputData!
            .JunctionPosition
            .Split('\n')
            .ToList();
        _coordinates = junctionPosition
            .Select((junction, index) => new KeyValuePair<int, string>(index, junction))
            .ToDictionary(); 
        
        int noOfJunctions = junctionPosition.Count;
        Dictionary<(int, int), long> distances = new();
        
        for (int i=0; i<noOfJunctions; i++)
        { 
            for (int j = i + 1; j < noOfJunctions; j++) 
            { 
                List<int> vector = junctionPosition[i]
                    .Split(",")
                    .Select(int.Parse)
                    .ToList();
                
                var v1 = new Vector3(vector[0], vector[1], vector[2]);
                vector = junctionPosition[j]
                    .Split(",")
                    .Select(int.Parse)
                    .ToList(); 
                var v2 = new Vector3(vector[0], vector[1], vector[2]);
                
                long distance = (long) ( (v1.X -v2.X)*(v1.X -v2.X)
                                         + (v1.Y - v2.Y)*(v1.Y - v2.Y)
                                         + (v1.Z - v2.Z)* (v1.Z - v2.Z));
                
                distances = distances.SetDistance(i, j, distance);
            }
        }

        return distances;
    }

    private void GetCentralCircuit((int, int) circuit)
    {
        if (_part == Part.One)
            return;
        
        if (_circuits.Count > 1)
            return;
        string firstCoordinate = _coordinates[circuit.Item1];
        string secondCoordinate = _coordinates[circuit.Item2];
        
        _xCoordinateProduct = long.Parse(firstCoordinate.Split(",").First()) *
                             long.Parse(secondCoordinate.Split(",").First());
        
    }

    private void MergeCircuits((int, int) circuit)
    {
        List<int> firstCircuitGroup = _circuits.Single(c => c.Contains(circuit.Item1)); 
        List<int> secondCircuitGroup = _circuits.Single(c => c.Contains(circuit.Item2));
        int firstCircuitIndex = _circuits.IndexOf(firstCircuitGroup);
        List<int> mergedCircuit = [];
        mergedCircuit.AddRange(firstCircuitGroup);
        mergedCircuit.AddRange(secondCircuitGroup);
        _circuits[firstCircuitIndex] = mergedCircuit;
        _circuits.Remove(secondCircuitGroup);
    }
    
    private void GroupCircuits((int, int) circuits)
    {
        Func<List<int>, bool> predicate = c => c.Contains(circuits.Item1) || c.Contains(circuits.Item2); 
        int neighbourhoodCount = _circuits.Count(predicate);
        
        if (neighbourhoodCount > 1)
        {
            MergeCircuits(circuits);
            GetCentralCircuit(circuits);
            return;
        }
        
        List<int> matchingGroup = _circuits.Single(predicate);
        int index = _circuits.IndexOf(matchingGroup);
        matchingGroup.AddRange(circuits.Item1, circuits.Item2);
        _circuits[index] = matchingGroup.Distinct().ToList();

    }
    
    private void NearestNeighbours(Dictionary<(int, int), long> distances)
    {
        int connectionsToMake = _dataType == DataType.RealData ? 1000 : 10;

        List<KeyValuePair<(int, int), long>> sortedDistance = distances
            .ToList()
            .OrderBy(dist => dist.Value)
            .ToList();
        
        if (_part == Part.One)
            sortedDistance = sortedDistance.Take(connectionsToMake).ToList();
        
        _circuits = _coordinates.Keys.Select(key => new List<int>(){key}).ToList();
        
        foreach (var distance in sortedDistance)
        {
            GroupCircuits(distance.Key);
        }
        
        _circuits =  _circuits.Select(c => c.Distinct().ToList()).ToList();
    }
    
    public void Solve(Part part)
    {
        _part = part;
        if (_inputData == null) SetInputData(DataType.TestData);
        _distances = CalculateDistances();
        NearestNeighbours(_distances);
    }
}