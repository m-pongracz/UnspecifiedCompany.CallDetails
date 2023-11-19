using System.Globalization;
using CsvHelper;
using Giacom.CallDetails.Application.Exceptions;
using Giacom.CallDetails.Domain.CallDetails;
using Giacom.CallDetails.Persistence.CallDetails;

namespace Giacom.CallDetails.Application.CallDetails;

public class CallDetailService : ICallDetailService
{
    private readonly ICallDetailRepository _callDetailRepository;

    public CallDetailService(ICallDetailRepository callDetailRepository)
    {
        _callDetailRepository = callDetailRepository;
    }
    
    public async Task BulkInsertAsync<TData>(Stream csvStream, Func<TData, CallDetail> getCallDetail)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        await BulkInsertAsync(csv.GetRecords<TData>(), getCallDetail);
    }
    
    public Task BulkInsertAsync<TData>(IEnumerable<TData> records, Func<TData, CallDetail> getCallDetail)
    {
        return _callDetailRepository.BulkInsertAsync(records, getCallDetail);
    }
    
    public async Task<CallDetail> FindAsync(string reference)
    {
        var res = await _callDetailRepository.FindAsync(reference);

        if (res == null)
        {
            throw new NotFoundException($"Call detail with reference {reference} was not found.");
        }

        return res;
    }
    
    public Task<CountAndDurationResult> GetCountAndDurationAsync(DateOnly from, DateOnly to, CallType? type)
    {
        return _callDetailRepository.GetCountAndDurationAsync(from, to, type);
    }
}