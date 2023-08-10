using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.SeedWork;

namespace Utilities.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> AsSelectListItems<T>(
            this IEnumerable<T> entities,
            Func<T, string> textExpr,
            Func<T, object> sortExpr = null
        ) where T : Entity =>
            entities
                .OrderBy(sortExpr ?? textExpr)
                .Select(i => new SelectListItem(textExpr.Invoke(i), i.Id.ToString()));

        public static IEnumerable<SelectListItem> AsSelectListItems<T>(
            this IEnumerable<T> objects,
            Func<T, string> valueExpr,
            Func<T, string> textExpr,
            Func<T, object> sortExpr = null
        ) =>
            objects
                .OrderBy(sortExpr ?? textExpr)
                .Select(
                    i => new SelectListItem(textExpr.Invoke(i), valueExpr.Invoke(i))
                );

        public static string Join(this IEnumerable<object> objects, string separator = ", ") =>
            string.Join(separator, objects);
    }
}
