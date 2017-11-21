using Lexor.Utilities.SeedWork;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Utilities.SeedWork
{
    public abstract class ConfigurationBase
    {
        public virtual void ConfigureEntity<T>(EntityTypeBuilder<T> config) where T : Entity
        {
            config.Property(t => t.Id).IsRequired();
            config.HasKey(t => t.Id);
            config.Ignore(t => t.DomainEvents);
        }

        protected static string FkColName(Type entityType) => FkColName(entityType.Name);
        protected static string FkColName(string entityName) => $"{entityName}Id";

        protected static void DefineForeignKey<T>(EntityTypeBuilder<T> config, Type entityType) where T : Entity
        {
            var columnName = FkColName(entityType);

            config.Property<int>(columnName).IsRequired();
            config.HasIndex(columnName);
        }
    }
}
