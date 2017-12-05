using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Utilities.SeedWork
{
    // https://github.com/dotnet-architecture/eShopOnWeb
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}
