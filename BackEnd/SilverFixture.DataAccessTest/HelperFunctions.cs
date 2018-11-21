using System;
using System.Collections.Generic;
using System.Text;

namespace SilverFixture.DataAccessTest
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
