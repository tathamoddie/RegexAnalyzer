using System;
using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic
{
    static class StateChange<T> where T : struct
    {
        internal static Action<Stack<T>> RetainState = states => { };

        internal static Action<Stack<T>> PopState = states => states.Pop();

        internal static Action<Stack<T>> PushState(T state)
        {
            return states => states.Push(state);
        }

        internal static Action<Stack<T>> ReplaceState(T state)
        {
            return ReplaceState(state, 1);
        }

        internal static Action<Stack<T>> ReplaceState(T state, short pushCount)
        {
            if (pushCount <= 0)
                throw new ArgumentOutOfRangeException("pushCount", pushCount, "Must be a positive number.");

            return states =>
            {
                states.Pop();
                for (var i = 0; i < pushCount; i ++)
                    states.Push(state);
            };
        }
    }
}