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
            Func<T, string> valueExpr
        ) where T : Entity =>
            entities.Select(i => new SelectListItem(valueExpr.Invoke(i), i.Id.ToString()));
    }
}
