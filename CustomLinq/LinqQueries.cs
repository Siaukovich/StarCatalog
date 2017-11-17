using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLinq
{
    public static class LinqQueries
    {
        public static IEnumerable<T> Where<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            foreach (var element in collection)
            {
                if (predicate(element))
                    yield return element;
            }
        }

        public static int Count<T>(IEnumerable<T> collection)
        {
            int count = 0;
            foreach (var element in collection)
            {
                count++;
            }
            return count;
        }

        public static int Count<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            int count = 0;
            foreach (var element in collection)
            {
                if (predicate(element))
                    count++;
            }
            return count;
        }
    }
}
