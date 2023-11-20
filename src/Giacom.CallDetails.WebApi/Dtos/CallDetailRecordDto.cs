using Giacom.CallDetails.Domain.CallDetails;
using CsvColumnName = CsvHelper.Configuration.Attributes.NameAttribute;
using CsvOptional = CsvHelper.Configuration.Attributes.OptionalAttribute;
using CsvFormat = CsvHelper.Configuration.Attributes.FormatAttribute;

namespace Giacom.CallDetails.WebApi.Dtos;

/// <summary>
/// CDR DTO
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class CallDetailRecordDto
{
    /// <summary>
    /// Phone number of the caller
    /// </summary>
    [CsvColumnName("caller_id")]
    public string CallerId { get; set; } = null!;

    /// <summary>
    /// Phone number of the recipient
    /// </summary>
    [CsvColumnName("recipient")]
    public string Recipient { get; set; } = null!;

    /// <summary>
    /// Date on which the call was made
    /// </summary>
    [CsvFormat("dd/MM/yyyy")]
    [CsvColumnName("call_date")]
    public DateTime CallDate { get; set; }
    
    /// <summary>
    /// Time when the call ended
    /// </summary>
    [CsvColumnName("end_time")]
    public string EndTime { get; set; } = null!;
    
    /// <summary>
    /// Duration of the call in seconds
    /// </summary>
    [CsvColumnName("duration")]
    public int Duration { get; set; }
    
    /// <summary>
    /// The billable cost of the call -- to 3 decimal places (decipence)
    /// </summary>
    [CsvColumnName("cost")]
    public decimal Cost { get; set; }
    
    /// <summary>
    /// Unique reference for the call
    /// </summary>
    [CsvColumnName("reference")]
    public string Reference { get; set; } = null!;
    
    /// <summary>
    /// Currency for the cost -- ISO alpha-3
    /// </summary>
    [CsvColumnName("currency")]
    public string Currency { get; set; } = null!;

    /// <summary>
    /// Call Type, 1 = Domestic, 2 = International
    /// </summary>
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