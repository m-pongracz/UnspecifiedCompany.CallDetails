using Giacom.CallDetails.Domain;
using Giacom.CallDetails.Domain.CallDetails;

namespace Giacom.CallDetails.Application.CallDetails;

public interface ICallDetailService
{
    Task BulkInsertAsync<TData>(Stream csvStream, Func<TData, CallDetail> getCallDetail);
    
    Task BulkInsertAsync<TData>(IEnumerable<TData> records, Func<TData, CallDetail> getCallDetail);
    
    Task<CallDetail> FindAsync(string reference);
    
    Task<CountAndDurationResult> GetCountAndDurationAsync(DateOnly from, DateOnly to, CallType? type);
    
    Task<PagedResult<CallDetail>> GetAllForCallerAsync(PagingRequest paging, string callerId,
        DateOnly from, DateOnly to, CallType? type);
    
    Task<IEnumerable<CallDetail>> GetMostExpensiveForCallerAsync(string callerId, DateOnly from,
        DateOnly to, CallType? type, int count);
}