using Giacom.CallDetails.Domain.CallDetails;

namespace Giacom.CallDetails.Persistence.CallDetails;

public interface ICallDetailRepository
{
    Task BulkInsertAsync<TData>(IEnumerable<TData> records, Func<TData, CallDetail> getCallDetail);
    
    Task<CountAndDurationResult> GetCountAndDurationAsync(DateOnly from, DateOnly to, CallType? type);
    
    Task<CallDetail?> FindAsync(string reference);
}