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
    }
}
