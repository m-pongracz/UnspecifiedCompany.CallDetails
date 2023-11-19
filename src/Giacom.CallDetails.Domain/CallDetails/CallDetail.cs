namespace Giacom.CallDetails.Domain.CallDetails;

public class CallDetail
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Obsolete("For serialization purposes only")]
    public CallDetail()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public CallDetail(string callDetailId, string callerId, string recipient, DateOnly callDate, string endTime, int duration,
        decimal cost, string currency, CallType callType)
    {
        CallerId = callerId;
        Recipient = recipient;
        CallDate = callDate;
        EndTime = endTime;
        Duration = duration;
        Cost = cost;
        CallDetailId = callDetailId;
        Currency = currency;
        CallType = callType;
    }

    public string CallerId { get; set; }
    
    public string Recipient { get; set; }
    
    public DateOnly CallDate { get; set; }
    
    public string EndTime { get; set; }
    
    public int Duration { get; set; }
    
    public decimal Cost { get; set; }
    
    public string CallDetailId { get; set; }
    
    public string Currency { get; set; }
    
    public CallType CallType { get; set; }
}