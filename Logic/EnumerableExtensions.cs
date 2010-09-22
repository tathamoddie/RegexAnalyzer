using System.Collections.Generic;
using System.Linq;

namespace TathamOddie.RegexAnalyzer.Logic
{
    static class EnumerableExtensions
    {
        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            return !source.Any();
        }
    }
}