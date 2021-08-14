using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraderBlotter.Api.Utilities
{
    public static class Extensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> @this, IEnumerable<T> toAdd)
        {
            foreach (var element in toAdd)
            {
                @this.Add(element);
            }
        }
    }
}
