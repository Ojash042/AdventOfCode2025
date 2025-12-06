using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode25.Solutions;

public class DaySix : ISolutions<long>
{
    private readonly List<long> _columnResults = [];
    private char _currentOperation = '\0';
    private DaySixInput? _inputData;
    public long GetResults()
    {
        return _columnResults.Sum();
    }

    public void SetInputData(DataType dataType)
    {
        string arithmetic;
        
        string savePath = Path.Combine(AppContext.BaseDirectory, "Inputs");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(savePath)
            .AddJsonFile("DaySixInputs.json", optional:false, reloadOnChange:true)
            .Build();

        switch (dataType)
        { 
            default:
            case DataType.TestData:
                arithmetic = configuration["testData"] ?? string.Empty; 
                break;
                
            case DataType.RealData:
                arithmetic = configuration["realData"] ?? string.Empty;
                break;
        }

        _inputData = new DaySixInput(arithmetic);
    }

    private static List<long> GetNthColumn(CsvReader reader, int n)
    {
        
        List<long> nthColumnItems = [];
        
        while (reader.Read())
        {
            int value = reader.GetField<int>(n);
            nthColumnItems.Add(value);
        }

        return nthColumnItems;
    }

    private static long Product(List<long> operands)
    {
        int partitionPoint = operands.Count / 2;
        return operands.Count switch
        {
            1 => operands.First(),
            2 => operands.First() * operands.Last(),
            _ => Product(operands[..partitionPoint]) * Product(operands[(partitionPoint )..])
        };
    }

    private long CalculateColumn(List<long> operands, char operatorChar)
    {
        return operatorChar switch
        {
            '*' => Product(operands),
            '+' => operands.Sum(),
            _ => 0
        };
    }

    private void ParseInput()
    {
        Dictionary<int, List<long>> verticalNumbers = new ();
        List<string> operandList = _inputData!.Arithmetic
            .Split('\n')[..^1]
            .ToList();
        
        string csvOperands = string.Join("\n", operandList.
            Select(line => Regex.Replace(line, @"[ \t]+", ","))
            .ToList());
        
        int columns =  operandList.First().Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = ",",
            NewLine = "\n",
            BadDataFound = null,
            MissingFieldFound = null
        };
        
        for (int i = 0; i < columns; i++)
        {
            using var reader = new StringReader(csvOperands);
            using var csv = new CsvReader(reader, config);
            verticalNumbers[i] = GetNthColumn(csv, i);
        }

        string operators =  _inputData!.Arithmetic.Split('\n').Last();
        List<char> operatorList = operators.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(operatorChar => operatorChar[0])
            .ToList();

        for (int i = 0; i < operatorList.Count; i++)
        {
            long result = CalculateColumn(verticalNumbers[i], operatorList.ElementAt(i));
            _columnResults.Add(result);
        }
        
    }

    private void CephalopodParser()
    {
        List<string> operandList = _inputData!.Arithmetic
            .Split('\n')[..^1]
            .ToList();
        
        List<char> operators = _inputData!.Arithmetic
            .Split('\n')
            .Last()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(operatorChar => operatorChar[0])
            .ToList();

        int currentOperationCount = 0;
        _currentOperation = operators.ElementAt(currentOperationCount);
        List<long> currentOperands = [];
        
        int totalOperations = operandList.First().Length;
        for (int operationCount = 0; operationCount < totalOperations; operationCount++)
        {
            int count = operationCount;
            List<string> cephalopodNum = operandList.Select(line => 
                line.ElementAt(count).ToString()
            ).ToList();
            
            bool changeOperationCondition = cephalopodNum.All(num => num == " ");
            
            if ( changeOperationCondition)
            {
                long result =  CalculateColumn(currentOperands, _currentOperation);
                _columnResults.Add(result);
                currentOperationCount = Math.Min(++currentOperationCount, operators.Count);
                _currentOperation = operators.ElementAt(currentOperationCount);
                currentOperands.Clear();
                continue;
            }
            
            long number = long.Parse(string.Join("", cephalopodNum));
            currentOperands.Add(number);
        }
        
        long finalResult = CalculateColumn(currentOperands, _currentOperation);
        _columnResults.Add(finalResult);
    }

    public void Solve(Part part)
    {
        switch (part)
        {
            default:
            case Part.One:
                ParseInput();
                break;
            case Part.Two:
                CephalopodParser();
                break;
        }

    }
}