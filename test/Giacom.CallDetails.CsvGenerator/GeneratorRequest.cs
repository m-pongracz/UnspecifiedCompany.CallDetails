namespace Giacom.CallDetails.CsvGenerator;

public class GeneratorRequest
{
    public int Rows { get; }
    public string? CallerId { get; }

    public GeneratorRequest(int rows, string? callerId = null)
    {
        Rows = rows;
        CallerId = callerId;
    }
}