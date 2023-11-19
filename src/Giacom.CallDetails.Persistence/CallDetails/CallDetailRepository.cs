using Giacom.CallDetails.Domain.CallDetails;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using Giacom.CallDetails.Domain.CallDetails.Queries;

namespace Giacom.CallDetails.Persistence.CallDetails;

public class CallDetailRepository : EfRepositoryBase<string, CallDetail>, ICallDetailRepository
    {
    public CallDetailRepository(CallDetailsDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task BulkInsertAsync<TData>(IEnumerable<TData> records, Func<TData, CallDetail> getCallDetail)
    {
        const int batchSize = 2000;

        // ReSharper disable once RedundantAssignment
        var batch = new CallDetail[batchSize];
        
        while (true)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            batch = records.Take(batchSize).Select(getCallDetail).ToArray();   

            // TODO think about retry on failure
            await DbContext.BulkInsertAsync(batch);

            if (batch.Length < batchSize)
            {
                break;
            }
        }
    }

    public Task<CallDetail?> FindAsync(string reference)
    {
        return Entities.SingleOrDefaultAsync(new GetByReferenceQuery(reference).Create());
    }
}