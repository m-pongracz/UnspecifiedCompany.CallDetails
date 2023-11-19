using System.Linq.Expressions;

namespace Giacom.CallDetails.Domain.CallDetails.Queries;

public class AllInPeriodQuery : TimePeriodQuery
{
    private readonly CallType? _type;

    public AllInPeriodQuery(CallType? type, DateOnly from, DateOnly to)
        : base(from, to)
    {
        _type = type;
    }
    
    public override Expression<Func<CallDetail, bool>> Create()
    {
        var expression = base.Create();

        return !_type.HasValue ? expression : expression.And(x => x.CallType == _type);
    }
}