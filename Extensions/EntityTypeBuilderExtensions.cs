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

        public static void DefineForeignKey<T>(this EntityTypeBuilder<T> config, Type entityType, bool required = true) where T : Entity
        {
            var columnName = $"{entityType.Name}Id";

            config.Property(columnName).IsRequired(required);
            config.HasIndex(columnName);

            var deleteBehavior = DeleteBehavior.Cascade;

            // Optional entries: set to null on delete
            if (!required)
                deleteBehavior = DeleteBehavior.SetNull;

            // Force enums to Restrict always
            if (typeof(Enumeration).IsAssignableFrom(entityType))
                deleteBehavior = DeleteBehavior.Restrict;

            // Change FK delete behaviour if not Cascade (the default)
            var foreignKeys = config.Metadata.GetForeignKeys()
                .Where(fk => fk.PrincipalEntityType.ClrType == entityType);
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = deleteBehavior;
            }
        }
    }
}
