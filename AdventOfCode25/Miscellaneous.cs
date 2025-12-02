namespace AdventOfCode25;

public interface ISolutions<out T>
{
    public T GetResults();
    public void SetInputData(DataType dataType);
    public void Solve(Part part = Part.One){}
}

public enum Part
{
    One,
    Two
}


public enum DataType 
{
    TestData,
    RealData
}