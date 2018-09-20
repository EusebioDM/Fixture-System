using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.DataAccess
{
    internal static class HelperFunctions<Entity> where Entity : class
    {
        public static object GetKey(EntityEntry entry)
        {
            object keyValue = null;
            foreach (var propety in entry.Properties)
            {
                if (propety.Metadata.IsPrimaryKey())
                {
                    return propety.CurrentValue;
                }
            }
            return keyValue;
        }

        public static bool EntriesAreEqual(Context context, Entity first, Entity second)
        {
            EntityEntry firstEntry = context.Entry(first);
            EntityEntry secondEntry = context.Entry(second);

            return EntriesAreEqual(firstEntry, secondEntry);
        }

        public static bool EntriesAreEqual(EntityEntry first, EntityEntry second)
        {
            var firstKey = GetKey(first);
            var secondKey = GetKey(second);

            return firstKey.Equals(secondKey);
        }
    }
}
