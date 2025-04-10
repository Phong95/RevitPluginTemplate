using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin.Extensions
{
    public static class ArrayExtensions
    {
        public static List<List<T>> SplitListIntoNLists<T>(this List<T> list, int n)
        {
            int chunkSize = (int)Math.Ceiling((double)list.Count / n);

            return Enumerable.Range(0, n)
                .Select(i => list.Skip(i * chunkSize).Take(chunkSize).ToList())
                .ToList();
        }
    }
}
