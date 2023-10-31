using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BaseDeProjetos.Helpers
{
    public static class ListExtensions
    {
        public static decimal Median<T>(this List<T> list, Expression<Func<T, decimal>> selector)
        {
            // Compile the selector expression to a delegate
            Func<T, decimal> compiledSelector = selector.Compile();

            // Extract values to a new list
            List<decimal> values = list.Select(compiledSelector).ToList();

            // Sort the list
            values.Sort();

            // Compute median
            int count = values.Count;
            if (count == 0)
            {
                throw new InvalidOperationException("The list is empty.");
            }

            if (count % 2 == 0)
            {
                return (values[(count / 2) - 1] + values[count / 2]) / 2;
            }
            else
            {
                return values[count / 2];
            }
        }
    }
}