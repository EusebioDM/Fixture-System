using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.GenericEntityRepository
{
    internal class EntityUpdater<Entity> where Entity : class
    {
        private readonly IDesignTimeDbContextFactory<DbContext> contextFactory;

        public EntityUpdater(IDesignTimeDbContextFactory<DbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void UpdateGraph(Entity entityToUpdate)
        {
            Queue<object> entitiesLeftToUpdate = new Queue<object>();
            HashSet<EntityKeys> entitiesThatShouldBeInUpdate = new HashSet<EntityKeys>();
            entitiesLeftToUpdate.Enqueue(entityToUpdate);

            while (entitiesLeftToUpdate.Count() > 0)
            {
                UpdateRootEntityAndItsChildrenIfPossible(entitiesLeftToUpdate, entitiesThatShouldBeInUpdate);
            }

            RemoveNoLongerPresentEntities(entityToUpdate, entitiesThatShouldBeInUpdate);
        }

        private void UpdateRootEntityAndItsChildrenIfPossible(Queue<object> entitiesLeftToUpdate, HashSet<EntityKeys> entitiesThatShouldBeInUpdate)
        {
            object rootEntityToUpdate = entitiesLeftToUpdate.Peek();

            using (DbContext context = contextFactory.CreateDbContext(new string[0]))
            {
                TraverseEntityGraphUpdatingWhenPossible(entitiesLeftToUpdate, rootEntityToUpdate, context, entitiesThatShouldBeInUpdate);
                context.SaveChanges();
            }
            entitiesLeftToUpdate.Dequeue();
        }

        private void TraverseEntityGraphUpdatingWhenPossible(Queue<object> entitiesLeftToUpdate, object rootEntityToUpdate, DbContext context, HashSet<EntityKeys> entitiesThatShouldBeInUpdate)
        {
            Action<EntityEntryGraphNode> updateNodeRecursivelyAction = n => UpdateNodeRecursively(context, entitiesLeftToUpdate, n, entitiesThatShouldBeInUpdate);

            context.ChangeTracker.TrackGraph(rootEntityToUpdate, updateNodeRecursivelyAction);
        }

        private void UpdateNodeRecursively(DbContext context, Queue<object> toUpdateQueue, EntityEntryGraphNode node, HashSet<EntityKeys> set)
        {
            EntityEntry current = node.Entry;
            EntityEntry fatherNode = node.SourceEntry;
            set.Add(HelperFunctions<Entity>.GetKeys(current));

            if (EntryExistsInChangeTracker(context, current)) // Entity is already being tracked in a different node so the current context cant track it
            {
                EnqueueFatherNodeToLeftToUpdateQueue(toUpdateQueue, fatherNode);
            }
            else
            {
                SetEntityAsModifiedOrAdded(context, current, node);
            }
        }

        private void SetEntityAsModifiedOrAdded(DbContext context, EntityEntry entry, EntityEntryGraphNode node)
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

        private bool EntryExistsInDb(DbContext context, EntityEntry entry)
        {
            bool exists = entry.GetDatabaseValues() != null;
            return exists;
        }

        private bool EntryExistsInChangeTracker(DbContext context, EntityEntry entry)
        {
            IEnumerable<EntityEntry> entriesInChangeTracker = context.ChangeTracker.Entries();
            bool exists = entriesInChangeTracker.Any(e => HelperFunctions<Entity>.EntriesAreEqual(e, entry));
            return exists;
        }



        private void RemoveNoLongerPresentEntities(Entity entity, HashSet<EntityKeys> entitiesThatShouldBeInUpdate)
        {
            using (DbContext context = contextFactory.CreateDbContext(new string[0]))
            {
                EntityEntry entry = context.Entry(entity);
                EntityKeys key = HelperFunctions<Entity>.GetKeys(entry);
                Entity root = context.Find<Entity>(key.Keys.ToArray());
                RemoveEntitiesNotInUpdateRecusively(context, context.Entry(root), entitiesThatShouldBeInUpdate, new HashSet<EntityKeys>());
                context.SaveChanges();
            }
        }

        private void RemoveEntitiesNotInUpdateRecusively(DbContext context, EntityEntry currentEntry, HashSet<EntityKeys> entitiesThatShouldBeInUpdate, HashSet<EntityKeys> alreadyTraversed)
        {
            EntityKeys keys = HelperFunctions<Entity>.GetKeys(currentEntry);
            if (!alreadyTraversed.Contains(keys))
            {
                alreadyTraversed.Add(keys);
                foreach (var property in currentEntry.Navigations)
                {
                    property.Load();
                    dynamic propertyCurrentValue = property.CurrentValue;
                    if (property.Metadata.IsCollection())
                    {
                        RemoveEntitiesFromCollectionThatWereNotPartOftheUpdateAndCallRecursively(context, entitiesThatShouldBeInUpdate, propertyCurrentValue, alreadyTraversed);
                    }
                    else
                    {
                        CallThisMethodRecusivelyWithChildEntity(context, entitiesThatShouldBeInUpdate, property, alreadyTraversed);
                    }
                }
            }
        }

        private void CallThisMethodRecusivelyWithChildEntity(DbContext context, HashSet<EntityKeys> entitiesThatShouldBeInUpdate, NavigationEntry property, HashSet<EntityKeys> alreadyTraversed)
        {
            EntityEntry entry = context.Entry(property.CurrentValue);
            RemoveEntitiesNotInUpdateRecusively(context, entry, entitiesThatShouldBeInUpdate, alreadyTraversed);
        }

        private void RemoveEntitiesFromCollectionThatWereNotPartOftheUpdateAndCallRecursively(DbContext context, HashSet<EntityKeys> entitiesThatShouldBeInUpdate, dynamic entitiesThatNeedToBeFiltered, HashSet<EntityKeys> alreadyTraversed)
        {
            List<dynamic> toBeDeleted = new List<dynamic>();
            foreach (dynamic entity in entitiesThatNeedToBeFiltered)
            {
                EntityEntry entry = context.Entry(entity);
                EntityKeys entityKey = HelperFunctions<Entity>.GetKeys(entry);
                if (!entitiesThatShouldBeInUpdate.Contains(entityKey))
                {
                    toBeDeleted.Add(entity);
                }
                RemoveEntitiesNotInUpdateRecusively(context, entry, entitiesThatShouldBeInUpdate, alreadyTraversed);
            }
            Action<dynamic> deleteFromCollectionOfEntities = e => entitiesThatNeedToBeFiltered.Remove(e);
            toBeDeleted.ForEach(deleteFromCollectionOfEntities);
        }
    }
}
