using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilverFixture.GenericEntityRepository
{
    internal static class HelperFunctions<Entity> where Entity : class
    {
        public static EntityKeys GetKeys(EntityEntry entry)
        {
            EntityKeys keys = new EntityKeys();
            foreach (var propety in entry.Properties)
            {
                if (propety.Metadata.IsPrimaryKey())
                {
                    keys.AddKey(propety.CurrentValue);
                }
            }
            return keys;
        }

        public static bool EntriesAreEqual(DbContext context, Entity first, Entity second)
        {
            EntityEntry firstEntry = context.Entry(first);
            EntityEntry secondEntry = context.Entry(second);

            return EntriesAreEqual(firstEntry, secondEntry);
        }

        public static bool EntriesAreEqual(EntityEntry first, EntityEntry second)
        {
            EntityKeys firstKey = GetKeys(first);
            EntityKeys secondKey = GetKeys(second);

            return firstKey.Equals(secondKey);
        }
    }
}
