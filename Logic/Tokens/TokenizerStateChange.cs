using System;
using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic.Tokens
{
    static class TokenizerStateChange
    {
        internal static Action<Stack<TokenizerState>> RetainState = states => { };

        internal static Action<Stack<TokenizerState>> PopState = states => states.Pop();

        internal static Action<Stack<TokenizerState>> PushState(TokenizerState state)
        {
            return states => states.Push(state);
        }

        internal static Action<Stack<TokenizerState>> ReplaceState(TokenizerState state)
        {
            return ReplaceState(state, 1);
        }

        internal static Action<Stack<TokenizerState>> ReplaceState(TokenizerState state, short pushCount)
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