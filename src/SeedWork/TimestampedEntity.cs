using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.SeedWork
{
    public abstract class TimestampedEntity : Entity
    {
        public void SetTimestampInfo(string updatedBy, EntityState entityState)
        {
            var now = DateTime.Now;
            UpdatedAt = now;
            UpdatedBy = updatedBy;
            if (entityState == EntityState.Added)
            {
                CreatedAt = now;
                CreatedBy = updatedBy;
            }
        }

        public DateTime? CreatedAt { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string UpdatedBy { get; set; }

        #region Triggers

        public class ExternallyUpdatedAttribute : Attribute { }

        public static async Task EnsureTriggersExistAsync(DbContext context)
        {
            var entitiesNeedingTriggers = context.Model.GetEntityTypes()
                .Where(e => e.ClrType.IsSubclassOf(typeof(TimestampedEntity)) &&
                            Attribute.GetCustomAttributes(e.ClrType).Any(a => a is ExternallyUpdatedAttribute)
                );
            foreach (var entity in entitiesNeedingTriggers)
            {
                await EnsureTriggerExists(context, entity);
            }
        }

        private static async Task EnsureTriggerExists(DbContext context, IEntityType entity)
        {
            var cmds = GetTriggerSql(entity.GetTableName());
            foreach (var cmd in cmds)
            {
                await context.Database.ExecuteSqlRawAsync(cmd);
            }
        }

        private static IEnumerable<string> GetTriggerSql(string tableName)
        {
            var triggerNameBase = $"{tableName}_A";
            return new[] {
                $"drop trigger if exists {triggerNameBase}I",
                $"drop trigger if exists {triggerNameBase}U",
                $@"
create trigger {triggerNameBase}I on {tableName}
after insert
as
    declare @now datetime
    declare @user varchar(max)
    set @now = getdate()
    set @user = suser_name()
    update t 
    set UpdatedAt = @now,
        UpdatedBy = @user,
        CreatedAt = @now,
        CreatedBy = @user
    from {tableName} t
    join inserted i on i.Id = t.Id
    where i.UpdatedAt is null
", $@"
create trigger {triggerNameBase}U on {tableName}
after update
as
    declare @now datetime
    declare @user varchar(max)
    set @now = getdate()
    set @user = suser_name()
    update t 
    set UpdatedAt = @now,
        UpdatedBy = @user
    from {tableName} t
    join inserted i on i.Id = t.Id
    join deleted d on d.Id = t.Id
    where i.UpdatedAt = d.UpdatedAt
" };
        }

        #endregion
    }
}