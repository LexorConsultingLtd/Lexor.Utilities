using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilities.SeedWork;

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

        // https://github.com/dotnet-architecture/eShopOnWeb
        public static IEnumerable<T> List<T>(this DbContext context, ISpecification<T> spec) where T : class
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult
                .Where(spec.Criteria)
                .AsEnumerable();
        }
    }
}


