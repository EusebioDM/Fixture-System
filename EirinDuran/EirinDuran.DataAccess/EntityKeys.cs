using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    internal class EntityKeys
    {
        public IEnumerable<object> Keys => keys;
        private ICollection<object> keys;

        public EntityKeys()
        {
            keys = new List<object>();
        }

        public EntityKeys(ICollection<object> keys)
        {
            this.keys = keys;
        }

        public void AddKey(object key) => keys.Add(key);

        public override bool Equals(object obj)
        {
            if (obj is EntityKeys other)
            {
                return Keys.SequenceEqual(other.Keys);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (object p in keys)
            {
                hashCode ^= p.GetHashCode(); // XOR is used to prevent Integer Overflow
            }

            return hashCode;
        }
    }
}
