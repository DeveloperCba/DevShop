using System.Linq.Expressions;

namespace DevShop.Core.Datas.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = left.Parameters[0];
        var visitor = new ParameterReplaceVisitor(right.Parameters[0], parameter);

        var body = Expression.AndAlso(left.Body, visitor.Visit(right.Body));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = left.Parameters[0];
        var visitor = new ParameterReplaceVisitor(right.Parameters[0], parameter);

        var body = Expression.OrElse(left.Body, visitor.Visit(right.Body));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
        var parameter = expression.Parameters[0];
        var visitor = new ParameterReplaceVisitor(parameter, Expression.Not(expression.Body));

        return Expression.Lambda<Func<T, bool>>(visitor.Visit(expression.Body), parameter);
    }

    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        return left.And(right);
    }

    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        return left.Or(right);
    }

    private class ParameterReplaceVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly Expression _newExpression;

        public ParameterReplaceVisitor(ParameterExpression oldParameter, Expression newExpression)
        {
            _oldParameter = oldParameter;
            _newExpression = newExpression;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newExpression : base.VisitParameter(node);
        }
    }
}