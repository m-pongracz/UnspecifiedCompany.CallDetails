namespace Giacom.CallDetails.Domain.CallDetails;

public class CountAndDurationResult
{
    public int Count { get; }
    public int TotalDuration { get; }
    

    public CountAndDurationResult(int count, int totalDuration)
    {
        Count = count;
        TotalDuration = totalDuration;
    }
}