using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Utilities.Extensions
{
    public static class SelectListItemExtensions
    {
        public static List<SelectListItem> NewListWithSelectItem(
            IEnumerable<SelectListItem> items,
            string selectItemText = "-- Select --"
        )
        {
            var result = new List<SelectListItem> { new SelectListItem(selectItemText, "") };
            result.AddRange(items);
            return result;
        }
    }
}
