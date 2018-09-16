using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    internal class EntityUpdater<Entity> where Entity : class
    {
        public void UpdateEntityWithItsChildren(Context context, Entity entity)
        {
            EntityEntry<Entity> entry = context.Entry<Entity>(entity);
            UpdateEntityRec(context, entry);
        }

        private void UpdateEntityRec(Context context, EntityEntry entry)
        {
            foreach (NavigationEntry property in entry.Navigations)
            {
                // The current value of a propety is returned as an object by EF so casting is needed to determine whether 
                // the propety is a single navigatable propety or a collection on navigatables
                try
                {
                    List<object> entries = TryToCastPropetyToCollectionOfEntries(context, property);
                    UpdateEntityCollection(context, entries);
                }
                catch (ArgumentException) // Casting failed since property is a single navigatable propety
                {
                    UpdateSingleEntry(context, property);
                }
            }
            try
            {
                context.Update(entry.Entity);
            }
            catch (InvalidOperationException) // Update failed since another instance of this entity is already present in context
            {
                if (!entry.IsKeySet)
                {
                    entry.State = EntityState.Unchanged;
                }
            }
        }

        private List<object> TryToCastPropetyToCollectionOfEntries(Context context, NavigationEntry property)
        {
            List<object> entries = (property.CurrentValue as IEnumerable<object>).Cast<object>().ToList();
            return entries;
        }

        private void UpdateEntityCollection(Context context, List<object> entries)
        {
            foreach (object obj in entries)
            {
                EntityEntry childEntry = context.Entry(obj);
                UpdateEntityRec(context, childEntry);
            }
        }

        private void UpdateSingleEntry(Context context, NavigationEntry property)
        {
            EntityEntry childEntry = context.Entry(property.CurrentValue);
            UpdateEntityRec(context, childEntry);
        }
    }
}
