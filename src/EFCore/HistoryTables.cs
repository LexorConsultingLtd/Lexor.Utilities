using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Reflection;
using Utilities.SeedWork;

namespace Utilities.EFCore
{
    public interface IHistoryEntity
    {
        Entity GetHistoryEntity();
    }

    public static class HistoryTables
    {
        private static readonly string HistoryEntityInterface = typeof(IHistoryEntity).FullName;

        private static bool InterfaceFilter(Type type, object obj)
            => type.ToString() == obj.ToString();

        public static void Process(ChangeTracker changeTracker)
        {
            try
            {
                var entries = changeTracker.Entries().ToList();
                if (entries.Count == 0) return;

                var typeFilter = new TypeFilter(InterfaceFilter);
                var entriesWithHistoryTable = entries
                    .Where(i => i.Entity.GetType().FindInterfaces(typeFilter, HistoryEntityInterface).Any());
                foreach (var entry in entriesWithHistoryTable)
                {
                    if (entry.State != EntityState.Modified) return; // i.e. Ignore adds and deletes
                    var entity = (IHistoryEntity)entry.Entity;
                    var historyEntity = entity.GetHistoryEntity();
                    changeTracker.Context.Add(historyEntity);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception processing {nameof(HistoryTables)}: {ex.Message}", ex);
            }
        }
    }
}
