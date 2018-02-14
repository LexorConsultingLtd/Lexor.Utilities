using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using Utilities.SeedWork;

namespace Utilities.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureEntity<T>(this EntityTypeBuilder<T> config, string tableName = null) where T : Entity
        {
            tableName = tableName ?? $"{typeof(T).Name}s";
            config.ToTable(tableName);
            config.Property(t => t.Id).IsRequired();
            config.HasKey(t => t.Id);
            config.Ignore(t => t.DomainEvents);
        }

        public static void DefineForeignKey<T>(this EntityTypeBuilder<T> config, Type entityType, bool restrictDeletes = false, bool required = true) where T : Entity
        {
            var columnName = $"{entityType.Name}Id";

            config.Property(columnName).IsRequired(required);
            config.HasIndex(columnName);

            if (restrictDeletes || typeof(Enumeration).IsAssignableFrom(entityType))
            {
                // Change FK delete behaviour to Restrict instead of Cascade (the default)
                var foreignKeys = config.Metadata.GetForeignKeys()
                    .Where(fk => fk.PrincipalEntityType.ClrType == entityType);
                foreach (var foreignKey in foreignKeys)
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }
    }
}
