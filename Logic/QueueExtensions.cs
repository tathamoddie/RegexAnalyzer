using System;
using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic
{
    static class QueueExtensions
    {
        internal static IEnumerable<T> DequeuePattern<T>(this Queue<T> queue, IEnumerable<PatternSegment<T>> patternSegments)
        {
            var dequeuedElements = new List<T>();

            foreach (var patternSegment in patternSegments)
            {
                var segment = patternSegment;
                dequeuedElements.AddRange(
                    queue.DequeueWhile(i => segment.Predicate(i), patternSegment.Count));
            }

            return dequeuedElements;
        }

        internal static IEnumerable<T> DequeueWhile<T>(this Queue<T> queue, Func<T, bool> predicate)
        {
            return DequeueWhile(queue, predicate, null);
        }

        internal static IEnumerable<T> DequeueWhile<T>(this Queue<T> queue, Func<T, bool> predicate, int limit)
        {
            return DequeueWhile(queue, predicate, (int?)limit);
        }

        static IEnumerable<T> DequeueWhile<T>(this Queue<T> queue, Func<T, bool> predicate, int? limit)
        {
            var dequeuedElements = new List<T>();

            while (
                (limit == null || dequeuedElements.Count < limit) &&
                queue.Count > 0 &&
                predicate(queue.Peek()))
                dequeuedElements.Add(queue.Dequeue());

            return dequeuedElements;
        }
    }
}