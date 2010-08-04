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
            return states =>
            {
                states.Pop();
                states.Push(state);
            };
        }
    }
}