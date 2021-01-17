using System;
using System.Linq;

namespace Core.Utils
{
    public static class Search
    {
        public static int IndexOfFirstGreatOrEqual<T1, T2>(T1[] source, Func<T1, T2> selector, T2 search) 
            where T2 : IComparable
        {
            var l = 0;
            var r = source.Length - 1;
            while (l < r - 1)
            {
                var currentIndex = l + (r - l) / 2;
                var current = selector(source[currentIndex]);
                var compare = current.CompareTo(search);
                
                if (compare > 0)
                {
                    r = currentIndex;
                }
                else if (compare < 0)
                {
                    l = currentIndex + 1;
                }
                else
                {
                    r = l = currentIndex;
                }
            }
            var lValue = selector(source[l]);
            var rValue = selector(source[r]);
            if (lValue.CompareTo(search) >= 0)
            {
                return l;
            }
            else if (rValue.CompareTo(search) >= 0)
            {
                return r;
            }
            return -1;
        }
    }
}