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

            // OrUpdate ensures that the batch wont fail if the record already exists in the database which will
            // be helpful in case the same file has to be processed multiple times (e.g. in case of a failure)
            await DbContext.BulkInsertOrUpdateAsync(batch);

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