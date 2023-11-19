using System.Linq.Expressions;

namespace Giacom.CallDetails.Domain;

public static class ExpressionExtensions
{
    public static Expression<Func<TModel, bool>> And<TModel>(this Expression<Func<TModel, bool>> left, Expression<Func<TModel, bool>> right)
    {
        var param = Expression.Parameter(typeof(TModel), "x");
        var body = Expression.AndAlso(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param)
        );
        return Expression.Lambda<Func<TModel, bool>>(body, param);
    }
}