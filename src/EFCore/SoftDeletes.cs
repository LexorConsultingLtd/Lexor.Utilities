﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace Utilities.EFCore
{
    public static class SoftDeletes
    {
        public const string SoftDeleteColumnName = "IsDeleted";

        public static void EnableSoftDeletes<T>(this EntityTypeBuilder<T> config) where T : class
        {
            config.Property<bool>(SoftDeleteColumnName);
            config.HasQueryFilter(t => !EF.Property<bool>(t, SoftDeleteColumnName));
        }

        public static void Process(ChangeTracker changeTracker)
        {
            try
            {
                var entries = changeTracker.Entries().ToList();
                if (entries.Count == 0) return;

                var softDeleteEntries = entries
                    .Where(e => e.Properties.Any(p => p.Metadata.Name.Equals(SoftDeleteColumnName)));
                foreach (var entry in softDeleteEntries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues[SoftDeleteColumnName] = false;
                            break;

                        case EntityState.Deleted:
                            entry.CurrentValues[SoftDeleteColumnName] = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception processing {nameof(SoftDeletes)}: {ex.Message}", ex);
            }
        }
    }
}
