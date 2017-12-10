using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Utilities.SeedWork
{
    // https://github.com/dotnet-architecture/eShopOnWeb
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria, int start = -1, int length = -1, int order = -1)
        {
            Criteria = criteria;
            Start = start;
            Length = length;
            Order = order;
        }

        public Expression<Func<T, bool>> Criteria { get; protected set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public int Start { get; }
        public int Length { get; }
        public int Order { get; }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        // string-based includes allow for including children of children, e.g. Basket.Items.Product
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
