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
            HashSet<object> set = new HashSet<object>();
            entitiesLeftToUpdate.Enqueue(entity);

            while (entitiesLeftToUpdate.Count() > 0)
            {
                UpdateRootEntityAndItsChildrenIfPossible(contextFactory, entitiesLeftToUpdate, set);
            }

            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                EntityEntry entry = context.Entry(entity);
                object key = HelperFunctions<Entity>.GetKey(entry);
                Entity root = context.Find<Entity>(key);
                //context.ChangeTracker.TrackGraph(root,n,f => Callback(n,set,f));
                //context.ChangeTracker.TrackGraph<Entity>(root, root,(n, f) => Callback(n, f, set));
                Rec(context, context.Entry(root), set);
                context.SaveChanges();
            }
        }

        private void Rec(Context context, EntityEntry entry, HashSet<object> set)
        {
            object key = HelperFunctions<Entity>.GetKey(entry);
            if (!set.Contains(key))
                entry.State = EntityState.Deleted;
            foreach (var p in entry.Navigations)
            {
                p.Load();
                try
                {
                    IEnumerable<object> l = (p.CurrentValue as IEnumerable<object>).Cast<object>().ToList();
                    foreach (object o in l)
                    {
                        var e = context.Entry(o);
                        Rec(context, e, set);
                    }
                }
                catch (Exception)
                {
                    var e = context.Entry(p.CurrentValue);
                    Rec(context, e, set);
                }
            }
        }

        private bool Callback(EntityEntryGraphNode node, Entity entity, HashSet<object> set)
        {
            object key = HelperFunctions<Entity>.GetKey(node.Entry);
            if (!set.Contains(key))
                node.Entry.State = EntityState.Deleted;
            return true;
        }

        private void UpdateRootEntityAndItsChildrenIfPossible(IDesignTimeDbContextFactory<Context> contextFactory, Queue<object> entitiesLeftToUpdate, HashSet<object> set)
        {
            object rootEntityToUpdate = entitiesLeftToUpdate.Peek();

            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                TraverseEntityGraphUpdatingWhenPossible(entitiesLeftToUpdate, rootEntityToUpdate, context, set);
                context.SaveChanges();
            }
            entitiesLeftToUpdate.Dequeue();
        }

        private void TraverseEntityGraphUpdatingWhenPossible(Queue<object> entitiesLeftToUpdate, object rootEntityToUpdate, Context context, HashSet<object> set)
        {
            Action<EntityEntryGraphNode> updateNodeRecursivelyAction = n => UpdateNodeRecursively(context, entitiesLeftToUpdate, n, set);

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
                //var db = context.Model.
                //foreach (var p in db.Collections)
                //{
                //    IEnumerable<object> dbCollection = (IEnumerable<object>)p.CurrentValue;
                //    IEnumerable<object> oCollection = (IEnumerable<object>)entry.Collection(p.Metadata.Name).CurrentValue;
                //    foreach (var obj in dbCollection.Where(o => !oCollection.Any(ob => HelperFunctions<object>.EntriesAreEqual(context, o, ob))))
                //    {
                //        context.Entry(obj).State = EntityState.Deleted;
                //    }
                //}

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
