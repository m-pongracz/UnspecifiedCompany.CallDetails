using System.Linq.Expressions;

namespace Giacom.CallDetails.Domain.CallDetails.Queries;

public class GetByReferenceQuery
{
    private readonly string _reference;

    public GetByReferenceQuery(string reference)
    {
        _reference = reference;
    }
    
    public Expression<Func<CallDetail, bool>> Create() => x => x.CallDetailId == _reference;  
}