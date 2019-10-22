using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GovUk.Education.ExploreEducationStatistics.Data.Model.Database
{
    public static class PredicateBuilder
    {   
        public static Expression<Func<T, R>> Expr<T, R>(Func<T, R> function)
        {
            return t => function(t);
        }
        
        public static Expression<Func<T, bool>> True<T>()
        {
            return input => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return input => false;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            return expression1.CombineWithOrElse(expression2);
                
//            var invocationExpression =
//                Expression.Invoke(expression2, expression1.Parameters);
//            return Expression.Lambda<Func<T, bool>>(
//                Expression.OrElse(expression1.Body, invocationExpression),
//                expression1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            return expression1.CombineWithAndAlso(expression2);

//            var invocationExpression = Expression.Invoke(expression2, expression1.Parameters);
//            return Expression.Lambda<Func<T, bool>>(
//                Expression.AndAlso(expression1.Body, invocationExpression),
//                expression1.Parameters);
        }
        private static Expression<Func<TInput, bool>> CombineWithAndAlso<TInput>(this Expression<Func<TInput, bool>> func1, Expression<Func<TInput, bool>> func2)
        {
            return Expression.Lambda<Func<TInput, bool>>(
                Expression.AndAlso(
                    func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        private static Expression<Func<TInput, bool>> CombineWithOrElse<TInput>(this Expression<Func<TInput, bool>> func1, Expression<Func<TInput, bool>> func2)
        {
            return Expression.Lambda<Func<TInput, bool>>(
                Expression.AndAlso(
                    func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        private class ExpressionParameterReplacer : ExpressionVisitor
        {
            public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
            {
                ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
                for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
                    ParameterReplacements.Add(fromParameters[i], toParameters[i]);
            }

            private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replacement;
                if (ParameterReplacements.TryGetValue(node, out replacement))
                    node = replacement;
                return base.VisitParameter(node);
            }
        }
    }
}