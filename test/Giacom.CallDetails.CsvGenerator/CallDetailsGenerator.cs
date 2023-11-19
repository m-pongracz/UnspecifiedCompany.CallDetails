using System.Globalization;
using System.Text;
using CsvHelper;
using Giacom.CallDetails.Domain.CallDetails;
using Giacom.CallDetails.WebApi.Dtos;

namespace Giacom.CallDetails.CsvGenerator;

public static class CallDetailsGenerator
{
    public static IEnumerable<CallDetailRecordDto> GenerateRows(GeneratorRequest request)
    {
        var random = new Random();
        
        for (var i = 0; i < request.Rows; i++)
        {
            var duration = random.Next(1, 1000);
            var callDate = DateTime.UtcNow.AddDays(-random.Next(1, 1000));
            
            yield return new CallDetailRecordDto()
            {
                Reference = Guid.NewGuid().ToString().Replace("-", "").ToUpper(),
                CallerId = request.CallerId ?? random.NextInt64().ToString(),
                Recipient = random.NextInt64().ToString(),
                CallDate = callDate,
                EndTime = callDate.AddSeconds(duration).ToString("HH:mm:ss"),
                Duration = duration,
                Cost = (decimal)random.NextDouble(),
                Currency = random.NextBool() ? "GBP" : "USD",
                CallType = random.NextBool() ? CallType.Domestic : CallType.International
            };
        }
    }
    
    public static async Task GenerateFile(GeneratorRequest request)
    {
        var stream = new FileStream(Guid.NewGuid()+".csv", FileMode.Create);
        await using var streamWriter = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);
        await using var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        writer.Context.RegisterClassMap<CallDetailRecordCsvMap>();
        await writer.WriteRecordsAsync(GenerateRows(request));
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