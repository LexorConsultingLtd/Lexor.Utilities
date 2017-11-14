using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lexor.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        // Need to invoke with "this" e.g. this.GetPrivateProperty(...)

        public static object GetPrivateProperty(this object instance, Type type, string propertyName)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = type.GetField(propertyName, bindFlags);
            return field?.GetValue(instance);
        }

        public static void SetPrivateProperty(this object instance, Type type, string propertyName, object value)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = type.GetField(propertyName, bindFlags);
            field?.SetValue(instance, value);
        }
    }
}
