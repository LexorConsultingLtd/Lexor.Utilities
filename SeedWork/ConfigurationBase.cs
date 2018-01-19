using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace Utilities.SeedWork
{
    public abstract class ConfigurationBase
    {
        public virtual void ConfigureEntity<T>(EntityTypeBuilder<T> config, string tableName = null) where T : Entity
        {
            tableName = tableName ?? $"{typeof(T).Name}s";
            config.ToTable(tableName);
            config.Property(t => t.Id).IsRequired();
            config.HasKey(t => t.Id);
            config.Ignore(t => t.DomainEvents);
        }

        protected static string FkColName(Type entityType) => FkColName(entityType.Name);
        protected static string FkColName(string entityName) => $"{entityName}Id";

        protected static void DefineForeignKey<T>(EntityTypeBuilder<T> config, Type entityType, bool restrictDeletes = false) where T : Entity
        {
            var columnName = FkColName(entityType);

            config.Property<int>(columnName).IsRequired();
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
