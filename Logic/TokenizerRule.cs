using System;
using System.Collections.Generic;
using System.Linq;

namespace TathamOddie.RegexAnalyzer.Logic
{
    class TokenizerRule
    {
        public const IEnumerable<string> AnyData = null;

        public static readonly IEnumerable<string> LetterData =
            Enumerable.Range('a', 'z').Concat(Enumerable.Range('A', 'Z')).Select(i => ((char)i).ToString());

        public static readonly IEnumerable<string> NumberData =
            Enumerable.Range('0', '9').Select(c => c.ToString());

        public static readonly IEnumerable<string> LetterAndNumberData =
            LetterData.Union(NumberData);

        readonly IEnumerable<TokenizerState> applicableStates;
        readonly IEnumerable<string> applicableData;
        readonly TokenType type;
        readonly Action<Stack<TokenizerState>> stateChange;

        public TokenizerRule(TokenizerState state, string applicableData, TokenType type, Action<Stack<TokenizerState>> stateChange)
            : this(new[] { state }, new[] { applicableData }, type, stateChange)
        {
        }

        public TokenizerRule(TokenizerState state, IEnumerable<string> applicableData, TokenType type, Action<Stack<TokenizerState>> stateChange)
            : this(new[] { state }, applicableData, type, stateChange)
        {
        }

        public TokenizerRule(IEnumerable<TokenizerState> applicableStates, string applicableData, TokenType type, Action<Stack<TokenizerState>> stateChange)
            : this(applicableStates, new[] { applicableData }, type, stateChange)
        {
        }

        public TokenizerRule(IEnumerable<TokenizerState> applicableStates, IEnumerable<string> applicableData, TokenType type, Action<Stack<TokenizerState>> stateChange)
        {
            this.applicableStates = applicableStates;
            this.applicableData = applicableData;
            this.type = type;
            this.stateChange = stateChange;
        }

        public IEnumerable<TokenizerState> ApplicableStates
        {
            get { return applicableStates; }
        }

        public IEnumerable<string> ApplicableData
        {
            get { return applicableData; }
        }

        public TokenType Type
        {
            get { return type; }
        }

        public Action<Stack<TokenizerState>> StateChange
        {
            get { return stateChange; }
        }
    }
}