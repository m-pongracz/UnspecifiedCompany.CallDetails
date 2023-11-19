using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Giacom.CallDetails.Domain.CallDetails.Queries;

public abstract class TimePeriodQuery
{
    private readonly DateOnly _to;
    private readonly DateOnly _from;

    protected TimePeriodQuery(DateOnly from, DateOnly to)
    {
        _from = from;
        _to = to;
        
        Validate();
    }
    
    public virtual Expression<Func<CallDetail, bool>> Create() => x => x.CallDate >= _from && x.CallDate <= _to;

    private void Validate()
    {
        var diff = _to.ToDateTime(TimeOnly.MinValue) - _from.ToDateTime(TimeOnly.MinValue);
        const int maxDayDiff = 30;
        
        switch (diff.TotalDays)
        {
            case > maxDayDiff:
                throw new ValidationException($"Time period cannot be greater than {maxDayDiff} days");
            case < 0:
                throw new ValidationException("Time period cannot be negative");
        }
    }
}