using System.Linq;
using System.Linq.Expressions;
using Utilities.DataTables;

namespace Utilities.Extensions
{
    // Inspiration: https://stackoverflow.com/a/36303246

    // ReSharper disable once InconsistentNaming
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, DataTablesAjaxPostModel model)
        {
            var expression = source.Expression;
            var firstSortField = true;
            foreach (var order in model.order)
            {
                var sortField = model.columns[order.column].data;
                var sortAscending = order.dir.ToLower().Equals("asc");

                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = GetSelector(parameter, sortField);
                var method = GetSortMethodName(sortAscending, firstSortField);
                expression = Expression.Call(
                    typeof(Queryable),
                    method,
                    new[] { source.ElementType, selector.Type },
                    expression,
                    Expression.Quote(Expression.Lambda(selector, parameter))
                );
                firstSortField = false;
            }
            return firstSortField ? source : source.Provider.CreateQuery<T>(expression);
        }

        private static Expression GetSelector(Expression parameter, string sortField)
        {
            var selector = sortField
                .Split(".")
                .Aggregate<string, Expression>(
                    null,
                    (current, fieldName) => Expression.PropertyOrField(current ?? parameter, fieldName)
                );
            return selector;
        }

        private static string GetSortMethodName(bool sortAscending, bool firstSortField)
        {
            var method = sortAscending
                ? firstSortField ? nameof(Queryable.OrderBy) : nameof(Queryable.ThenBy)
                : firstSortField ? nameof(Queryable.OrderByDescending) : nameof(Queryable.ThenByDescending);
            return method;
        }
    }
}
