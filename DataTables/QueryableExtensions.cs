using System.Linq;
using System.Linq.Expressions;

namespace Utilities.DataTables
{
    // Inspiration: https://stackoverflow.com/a/36303246
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, DataTablesAjaxPostModel model)
        {
            var expression = source.Expression;
            var firstSortField = true;
            foreach (var order in model.order)
            {
                var fieldName = model.columns[order.column].data;
                var sortAscending = order.dir.ToLower().Equals("asc");

                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = Expression.PropertyOrField(parameter, fieldName);
                var method = sortAscending
                    ? firstSortField ? nameof(Queryable.OrderBy) : nameof(Queryable.ThenBy)
                    : firstSortField ? nameof(Queryable.OrderByDescending) : nameof(Queryable.ThenByDescending);
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
    }
}
