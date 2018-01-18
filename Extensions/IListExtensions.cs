using System.Collections.Generic;
using System.Linq;
using Utilities.SeedWork;

namespace Utilities.Extensions
{
    public static class IListExtensions
    {
        public static void CopyValues<T>(this IList<T> list, IList<T> source) where T : Entity
        {
            // Remove entries that no longer exist
            var deletedItems = list.Where(item => source.All(s => s.Id != item.Id)).ToList();
            foreach (var deletedItem in deletedItems)
                list.Remove(deletedItem);

            // Update existing entries
            var existing = source.Where(i => !i.IsTransient());
            foreach (var sourceItem in existing)
            {
                var item = list.FirstOrDefault(i => i.Equals(sourceItem));
                if (item == null) throw new KeyNotFoundException("Unable to find item in source list");
                item.CopyValues(sourceItem);
            }

            // Add new entries
            var newItems = source.Where(item => item.IsTransient());
            foreach (var sourceItem in newItems)
                list.Add(sourceItem);
        }
    }
}
