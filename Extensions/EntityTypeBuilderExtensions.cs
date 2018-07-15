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
        }

        public static EntityTypeBuilder<T> DefineForeignKey<T>(
            this EntityTypeBuilder<T> config,
            Type entityType,
            bool required = true,
            string columnName = null,
            DeleteBehavior defaultDeleteBehavior = DeleteBehavior.Cascade
        ) where T : Entity
        {
            columnName = columnName ?? $"{entityType.Name}Id";

            config.Property(columnName).IsRequired(required);
            config.HasIndex(columnName);

            var deleteBehavior = defaultDeleteBehavior;

            // Optional entries: set to null on delete always
            if (!required)
                deleteBehavior = DeleteBehavior.SetNull;

            // Force enums to Restrict always
            if (typeof(Enumeration).IsAssignableFrom(entityType))
                deleteBehavior = DeleteBehavior.Restrict;

            // Change FK delete behaviour if not Cascade (the default)
            var foreignKey = config.Metadata.GetForeignKeys()
                .Single(i => i.Properties.Any(j => j.Name == columnName));
            foreignKey.DeleteBehavior = deleteBehavior;

            return config;
        }
    }
}
