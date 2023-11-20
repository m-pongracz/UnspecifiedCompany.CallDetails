using System.Globalization;
using System.Text;
using CsvHelper;
using Giacom.CallDetails.Domain.CallDetails;
using Giacom.CallDetails.WebApi.Dtos;

namespace Giacom.CallDetails.CsvGenerator;

public static class CallDetailsGenerator
{
    private static readonly Random Random = new ();
    
    public static CallDetailRecordDto GenerateRow(GeneratorRequest? request = null)
    {
        var duration = Random.Next(1, 1000);
        var callDate = request?.CallDate ?? DateTime.UtcNow.AddDays(-Random.Next(1, 1000));

        return new CallDetailRecordDto()
        {
            Reference = Guid.NewGuid().ToString().Replace("-", "").ToUpper(),
            CallerId = request?.CallerId ?? Random.NextInt64().ToString(),
            Recipient = Random.NextInt64().ToString(),
            CallDate = callDate,
            EndTime = callDate.AddSeconds(duration).ToString("HH:mm:ss"),
            Duration = request?.Duration ?? duration,
            Cost = (decimal)(request?.Cost ?? Random.NextDouble()),
            Currency = request?.Currency ?? (Random.NextBool() ? "GBP" : "USD"),
            CallType = request?.CallType ?? (Random.NextBool() ? CallType.Domestic : CallType.International)
        };
    }
    
    public static IEnumerable<CallDetailRecordDto> GenerateRows(int rows, GeneratorRequest? request = null)
    {
        for (var i = 0; i < rows; i++)
        {
            yield return GenerateRow(request);
        }
    }
    
    public static async Task GenerateFile(int rows, GeneratorRequest? request = null)
    {
        var stream = new FileStream(Guid.NewGuid()+".csv", FileMode.Create);
        await using var streamWriter = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);
        await using var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        writer.Context.RegisterClassMap<CallDetailRecordCsvMap>();
        await writer.WriteRecordsAsync(GenerateRows(rows, request));
    }
    
    public static Stream GenerateStream(IEnumerable<CallDetailRecordDto> rows)
    {
        var stream = new MemoryStream();
        using var streamWriter = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);
        using var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        writer.WriteRecords(rows);

        return stream;
    }
}