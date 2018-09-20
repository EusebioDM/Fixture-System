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
                UpdateRootEntityAndItsChildrenIfPossible(contextFactory, entitiesLeftToUpdate);
            }
        }

        private void UpdateRootEntityAndItsChildrenIfPossible(IDesignTimeDbContextFactory<Context> contextFactory, Queue<object> entitiesLeftToUpdate)
        {
            object rootEntityToUpdate = entitiesLeftToUpdate.Peek();
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                TraverseEntityGraphUpdatingWhenPossible(entitiesLeftToUpdate, rootEntityToUpdate, context);
                context.SaveChanges();
            }
            entitiesLeftToUpdate.Dequeue();
        }

        private void TraverseEntityGraphUpdatingWhenPossible(Queue<object> entitiesLeftToUpdate, object rootEntityToUpdate, Context context)
        {
            Action<EntityEntryGraphNode> updateNodeRecursivelyAction = n => UpdateNodeRecursively(context, entitiesLeftToUpdate, n);

            context.ChangeTracker.TrackGraph(rootEntityToUpdate, updateNodeRecursivelyAction);
        }

        private void UpdateNodeRecursively(Context context, Queue<object> toUpdateQueue, EntityEntryGraphNode node)
        {
            EntityEntry current = node.Entry;
            EntityEntry fatherNode = node.SourceEntry;

            if (EntryExistsInChangeTracker(context, current)) // Entity is already being tracked in a different node so the current context cant track it
            {
                EnqueueFatherNodeToLeftToUpdateQueue(toUpdateQueue, fatherNode);
            }
            else
            {
                SetEntityAsModifiedOrAdded(context, current);
            }
        }

        private void SetEntityAsModifiedOrAdded(Context context, EntityEntry entry)
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

        private void EnqueueFatherNodeToLeftToUpdateQueue(Queue<object> toUpdateQueue, EntityEntry fatherNode)
        {
            bool canEnqueueWithoutGettingStuckInLoop = !toUpdateQueue.Contains(fatherNode.Entity);
            if (canEnqueueWithoutGettingStuckInLoop)
            {
                toUpdateQueue.Enqueue(fatherNode.Entity); // Entity is added to the queue so it can be added in a different context
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
