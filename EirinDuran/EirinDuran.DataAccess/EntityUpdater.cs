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
        public void UpdateGraph(IDesignTimeDbContextFactory<Context> contextFactory, Entity entityToUpdate)
        {
            Queue<object> entitiesLeftToUpdate = new Queue<object>();
            HashSet<object> entitiesThatShouldBeInUpdate = new HashSet<object>();
            entitiesLeftToUpdate.Enqueue(entityToUpdate);

            while (entitiesLeftToUpdate.Count() > 0)
            {
                UpdateRootEntityAndItsChildrenIfPossible(contextFactory, entitiesLeftToUpdate, entitiesThatShouldBeInUpdate);
            }

            RemoveNoLongerPresentEntities(contextFactory, entityToUpdate, entitiesThatShouldBeInUpdate);
        }

        private void UpdateRootEntityAndItsChildrenIfPossible(IDesignTimeDbContextFactory<Context> contextFactory, Queue<object> entitiesLeftToUpdate, HashSet<object> entitiesThatShouldBeInUpdate)
        {
            object rootEntityToUpdate = entitiesLeftToUpdate.Peek();

            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                TraverseEntityGraphUpdatingWhenPossible(entitiesLeftToUpdate, rootEntityToUpdate, context, entitiesThatShouldBeInUpdate);
                context.SaveChanges();
            }
            entitiesLeftToUpdate.Dequeue();
        }

        private void TraverseEntityGraphUpdatingWhenPossible(Queue<object> entitiesLeftToUpdate, object rootEntityToUpdate, Context context, HashSet<object> entitiesThatShouldBeInUpdate)
        {
            Action<EntityEntryGraphNode> updateNodeRecursivelyAction = n => UpdateNodeRecursively(context, entitiesLeftToUpdate, n, entitiesThatShouldBeInUpdate);

            context.ChangeTracker.TrackGraph(rootEntityToUpdate, updateNodeRecursivelyAction);
        }

        private void UpdateNodeRecursively(Context context, Queue<object> toUpdateQueue, EntityEntryGraphNode node, HashSet<object> set)
        {
            EntityEntry current = node.Entry;
            EntityEntry fatherNode = node.SourceEntry;
            set.Add(HelperFunctions<Entity>.GetKey(current));

            if (EntryExistsInChangeTracker(context, current)) // Entity is already being tracked in a different node so the current context cant track it
            {
                EnqueueFatherNodeToLeftToUpdateQueue(toUpdateQueue, fatherNode);
            }
            else
            {
                SetEntityAsModifiedOrAdded(context, current, node);
            }
        }

        private void SetEntityAsModifiedOrAdded(Context context, EntityEntry entry, EntityEntryGraphNode node)
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



        private void RemoveNoLongerPresentEntities(IDesignTimeDbContextFactory<Context> contextFactory, Entity entity, HashSet<object> entitiesThatShouldBeInUpdate)
        {
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                EntityEntry entry = context.Entry(entity);
                object key = HelperFunctions<Entity>.GetKey(entry);
                Entity root = context.Find<Entity>(key);
                RemoveEntitiesNotInUpdateRecusively(context, context.Entry(root), entitiesThatShouldBeInUpdate);
                context.SaveChanges();
            }
        }

        private void RemoveEntitiesNotInUpdateRecusively(Context context, EntityEntry currentEntry, HashSet<object> entitiesThatShouldBeInUpdate)
        {
            foreach (var property in currentEntry.Navigations)
            {
                property.Load();
                dynamic propertyCurrentValue = property.CurrentValue;
                if (property.Metadata.IsCollection())
                {
                    RemoveEntitiesFromCollectionThatWereNotPartOftheUpdateAndCallRecursively(context, entitiesThatShouldBeInUpdate, propertyCurrentValue);
                }
                else
                {
                    CallThisMethodRecusivelyWithChildEntity(context, entitiesThatShouldBeInUpdate, property);
                }
            }
        }

        private void CallThisMethodRecusivelyWithChildEntity(Context context, HashSet<object> entitiesThatShouldBeInUpdate, NavigationEntry property)
        {
            EntityEntry entry = context.Entry(property.CurrentValue);
            RemoveEntitiesNotInUpdateRecusively(context, entry, entitiesThatShouldBeInUpdate);
        }

        private void RemoveEntitiesFromCollectionThatWereNotPartOftheUpdateAndCallRecursively(Context context, HashSet<object> entitiesThatShouldBeInUpdate, dynamic entitiesThatNeedToBeFiltered)
        {
            List<dynamic> toBeDeleted = new List<dynamic>();
            foreach (dynamic entity in entitiesThatNeedToBeFiltered)
            {
                EntityEntry entry = context.Entry(entity);
                object entityKey = HelperFunctions<Entity>.GetKey(entry);
                if (!entitiesThatShouldBeInUpdate.Contains(entityKey))
                {
                    toBeDeleted.Add(entity);
                }
                RemoveEntitiesNotInUpdateRecusively(context, entry, entitiesThatShouldBeInUpdate);
            }
            Action<dynamic> deleteFromCollectionOfEntities = e => entitiesThatNeedToBeFiltered.Remove(e);
            toBeDeleted.ForEach(deleteFromCollectionOfEntities);
        }
    }
}
