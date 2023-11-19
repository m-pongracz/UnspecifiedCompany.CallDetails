using Giacom.CallDetails.Domain.CallDetails;

namespace Giacom.CallDetails.WebApi.Dtos;

public class CallDetailCountAndDurationDto
{
    public int Count { get; set; }
    public int TotalDuration { get; set; }
    
    public CallDetailCountAndDurationDto(CountAndDurationResult countAndDurationResult)
    {
        Count = countAndDurationResult.Count;
        TotalDuration = countAndDurationResult.TotalDuration;
    }
}