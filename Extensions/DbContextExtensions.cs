using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Lexor.Utilities.Extensions
{
    public static class DbContextExtensions
    {
        public static void ApplyModelConfigurations(this DbContext context, ModelBuilder modelBuilder)
        {
            var configTypes = Assembly.GetAssembly(context.GetType())
                .GetTypes()
                .Where(t => !t.IsAbstract)
                .SelectMany(t => t.GetInterfaces().Select(i => new { ConfigClass = t, Interface = i }))
                .Where(c => c.Interface.IsGenericType && c.Interface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                .Select(c => new { c.ConfigClass, GenericType = c.Interface.GenericTypeArguments.FirstOrDefault() })
                .ToList();
            foreach (var configType in configTypes)
            {
                var config = Activator.CreateInstance(configType.ConfigClass);
                var method = typeof(ModelBuilder)
                    .GetMethod(nameof(modelBuilder.ApplyConfiguration))
                    .MakeGenericMethod(configType.GenericType);
                method.Invoke(modelBuilder, new[] { config });
            }
        }
    }
}


