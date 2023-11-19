using Giacom.CallDetails.Domain.CallDetails;
using CsvColumnName = CsvHelper.Configuration.Attributes.NameAttribute;
using CsvOptional = CsvHelper.Configuration.Attributes.OptionalAttribute;
using CsvFormat = CsvHelper.Configuration.Attributes.FormatAttribute;

namespace Giacom.CallDetails.WebApi.Dtos;

// ReSharper disable once ClassNeverInstantiated.Global
public class CallDetailRecordDto
{
    [CsvColumnName("caller_id")]
    public string CallerId { get; set; } = null!;

    [CsvColumnName("recipient")]
    public string Recipient { get; set; } = null!;

    [CsvFormat("dd/MM/yyyy")]
    [CsvColumnName("call_date")]
    public DateTime CallDate { get; set; }
    
    [CsvColumnName("end_time")]
    public string EndTime { get; set; } = null!;
    
    [CsvColumnName("duration")]
    public int Duration { get; set; }
    
    [CsvColumnName("cost")]
    public decimal Cost { get; set; }
    
    [CsvColumnName("reference")]
    public string Reference { get; set; } = null!;
    
    [CsvColumnName("currency")]
    public string Currency { get; set; } = null!;

    [CsvColumnName("type")] 
    [CsvOptional]
    public CallType CallType { get; set; } = CallType.Domestic;

    public CallDetail GetCallDetail()
    {
        return new CallDetail(Reference, CallerId, Recipient, DateOnly.FromDateTime(CallDate), EndTime, Duration, Cost, Currency, CallType);
    }

    public CallDetailRecordDto()
    {
        
    }
    
    public CallDetailRecordDto(CallDetail callDetail)
    {
        Reference = callDetail.CallDetailId;
        CallerId = callDetail.CallerId;
        Recipient = callDetail.Recipient;
        CallDate = callDetail.CallDate.ToDateTime(TimeOnly.MinValue);
        EndTime = callDetail.EndTime;
        Duration = callDetail.Duration;
        Cost = callDetail.Cost;
        Currency = callDetail.Currency;
        CallType = callDetail.CallType;
    }
}