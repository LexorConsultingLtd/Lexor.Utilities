using System.Collections.Generic;

namespace Lexor.Utilities.Routes
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
            }
            return new Dictionary<string, string> { { "id", route.ToString() } };
        }
    }
}
