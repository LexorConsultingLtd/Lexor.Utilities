using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace Utilities.Extensions
{
    public static class PageModelExtensions
    {
        public static RouteValueDictionary RouteFor(this PageModel pageModel, string key, object value)
        {
            return new RouteValueDictionary
            {
                { key, value }
            };
        }
    }
}
