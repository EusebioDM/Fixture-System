using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.DataAccessTest
{
    internal static class HelperFunctions<T>
    {
        public static bool CollectionsHaveSameElements(IEnumerable<T> first, IEnumerable<T> second)
        {
            HashSet<T> firstSet = new HashSet<T>(first);
            HashSet<T> secondSet = new HashSet<T>(second);

            return firstSet.SetEquals(secondSet);
        }
    }
}
