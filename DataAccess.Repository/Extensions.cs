using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DataAccess.Repository
{
    public static class Extensions
    {
        public static List<string> GetColumnNames(this IDataReader reader)
        {
            return Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
        }
        public static Collection<T> ToCollection<T>(this List<T> items)
        {
            Collection<T> collection = new Collection<T>();

            for (int i = 0; i < items.Count; i++)
            {
                collection.Add(items[i]);
            }

            return collection;
        }
    }
}
