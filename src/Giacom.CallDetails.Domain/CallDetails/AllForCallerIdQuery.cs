using System.Linq.Expressions;
using Giacom.CallDetails.Domain.CallDetails.Queries;

namespace Giacom.CallDetails.Domain.CallDetails;

public class AllForCallerIdQuery : AllInPeriodQuery
{
    private readonly string _callerId;
    private readonly string? _currency;

    public AllForCallerIdQuery(string callerId, CallType? type, DateOnly from, DateOnly to, string? currency) :
        base(type, from, to)
    {
        _callerId = callerId;
        _currency = currency;
    }

    public override Expression<Func<CallDetail, bool>> Create()
    {
        return base.Create().And(x => x.CallerId == _callerId && (_currency == null || x.Currency == _currency));
    }
}