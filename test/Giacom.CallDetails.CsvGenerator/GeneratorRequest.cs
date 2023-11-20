using Giacom.CallDetails.Domain.CallDetails;

namespace Giacom.CallDetails.CsvGenerator;

public class GeneratorRequest
{
    public string? CallerId { get; }
    public DateTime? CallDate { get; }
    public double? Cost { get; }
    public int? Duration { get; }
    public CallType? CallType { get; }
    public string? Currency { get; }

    public GeneratorRequest(string? callerId = null, DateTime? callDate = null, double? cost = null, int? duration = null,
        CallType? callType = null, string? currency = null)
    {
        CallerId = callerId;
        CallDate = callDate;
        Cost = cost;
        Duration = duration;
        CallType = callType;
        Currency = currency;
    }
}