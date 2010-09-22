using System;
using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic
{
    static class QueueExtensions
    {
        internal static IEnumerable<T> DequeueWhile<T>(this Queue<T> queue, Func<T, bool> predicate)
        {
            var dequeuedTokens = new List<T>();
            
            while (queue.Count > 0 && predicate(queue.Peek()))
                dequeuedTokens.Add(queue.Dequeue());

            return dequeuedTokens;
        }
    }
}