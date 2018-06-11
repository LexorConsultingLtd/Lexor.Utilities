using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Extensions
{
    public static class Enum<T> where T : struct, IConvertible
    {
        public static IEnumerable<SelectListItem> AsSelectListItems(string blankItemText = null)
        {
            CheckType();
            var items = new List<SelectListItem>();
            if (blankItemText != null) items.Add(new SelectListItem { Text = blankItemText });

            items.AddRange(
                Enum.GetNames(typeof(T))
                    .Select((item, index) => new SelectListItem
                    {
                        Text = item.SplitCamelCase(),
                        Value = index.ToString()
                    })
            );
            return items;
        }

        private static void CheckType()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentOutOfRangeException(typeof(T).Name, "Must be an Enum");
        }
    }
}
