using Giacom.CallDetails.Domain.CallDetails;

namespace Giacom.CallDetails.Persistence.CallDetails;

public interface ICallDetailRepository
{
    Task BulkInsertAsync<TData>(IEnumerable<TData> records, Func<TData, CallDetail> getCallDetail);
    
    Task<CallDetail?> FindAsync(string reference);
}