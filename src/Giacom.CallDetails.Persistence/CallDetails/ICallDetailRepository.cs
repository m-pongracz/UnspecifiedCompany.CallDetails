using Giacom.CallDetails.Domain;
using Giacom.CallDetails.Domain.CallDetails;

namespace Giacom.CallDetails.Persistence.CallDetails;

public interface ICallDetailRepository
{
    Task BulkInsertAsync<TData>(IEnumerable<TData> records, Func<TData, CallDetail> getCallDetail);
    
    Task<CountAndDurationResult> GetCountAndDurationAsync(DateOnly from, DateOnly to, CallType? type);
    
    Task<CallDetail?> FindAsync(string reference);
    
    Task<PagedResult<CallDetail>> GetAllForCallerAsync(PagingRequest paging, string callerId, DateOnly from, DateOnly to, CallType? type);
    
    Task<IEnumerable<CallDetail>> GetMostExpensiveForCallerAsync(string callerId, DateOnly from,
        DateOnly to, CallType? type, string currency, int count);
}