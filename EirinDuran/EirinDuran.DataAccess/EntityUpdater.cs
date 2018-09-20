using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    internal class EntityUpdater<Entity> where Entity : class
    {
        public void UpdateGraph(IDesignTimeDbContextFactory<Context> contextFactory, Entity entity)
        {
            Queue<object> entitiesLeftToUpdate = new Queue<object>();
            entitiesLeftToUpdate.Enqueue(entity);

            while (entitiesLeftToUpdate.Count() > 0)
            {
                object next = entitiesLeftToUpdate.Peek();
                using (Context context = contextFactory.CreateDbContext(new string[0]))
                {
                    context.ChangeTracker.TrackGraph(next, n => UpdateNode(context, entitiesLeftToUpdate, n));
                    context.SaveChanges();
                }
                entitiesLeftToUpdate.Dequeue();
            }
        }

        private void UpdateNode(Context context, Queue<object> leftToUpdate, EntityEntryGraphNode n)
        {
            var entry = n.Entry;
            var sEntry = n.SourceEntry;
            if (!EntryExistsInChangeTracker(context, entry))
            {
                if (EntryExistsInDb(context, entry))
                {
                    entry.State = EntityState.Modified;
                }
                else
                {
                    entry.State = EntityState.Added;
                }
            }
            else
            {
                if (!leftToUpdate.Contains(sEntry.Entity))
                    leftToUpdate.Enqueue(sEntry.Entity);
            }
        }

        private bool EntryExistsInDb(Context context, EntityEntry entry)
        {
            bool exists = entry.GetDatabaseValues() != null;
            return exists;
        }

        private bool EntryExistsInChangeTracker(Context context, EntityEntry entry)
        {
            IEnumerable<EntityEntry> entriesInChangeTracker = context.ChangeTracker.Entries();
            bool exists = entriesInChangeTracker.Any(e => HelperFunctions<Entity>.EntriesAreEqual(e, entry));
            return exists;
        }
    }
}
