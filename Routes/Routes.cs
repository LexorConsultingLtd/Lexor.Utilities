using System.Collections.Generic;
using Utilities.SeedWork;

namespace Utilities.Routes
{
    public static class Routes
    {
        public static IDictionary<string, string> GetRouteValues(object route)
        {
            switch (route)
            {
                case null:
                    return null;
                case IDictionary<string, string> values:
                    return values;
                case Entity entity:
                    return GetRouteValues(entity.Id);
            }
            return new Dictionary<string, string> { { "id", route.ToString() } };
        }
    }
}
