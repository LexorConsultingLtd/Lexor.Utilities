using System;
using System.Reflection;

namespace Utilities.Extensions
{
    public static class ObjectExtensions
    {
        // Need to invoke with "this" e.g. this.GetPrivateProperty(...)

        public static object GetPrivateProperty(this object instance, Type type, string propertyName)
        {
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = type.GetField(propertyName, bindFlags);
            return field?.GetValue(instance);
        }

        public static void SetPrivateProperty(this object instance, Type type, string propertyName, object value)
        {
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = type.GetField(propertyName, bindFlags);
            field?.SetValue(instance, value);
        }
    }
}
