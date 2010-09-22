using System;
using System.Collections.Generic;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class TreeBuilderRule
    {
        public const IEnumerable<TreeBuilderState> AnyState = null;

        readonly IEnumerable<TreeBuilderState> states;
        readonly TokenType tokenType;
        readonly Func<Token, Queue<Token>, Node> nodeBuilder;
        readonly Action<Stack<TreeBuilderState>> stateChange;

        public TreeBuilderRule(TreeBuilderState state, TokenType tokenType, Func<Token, Queue<Token>, Node> nodeBuilder)
            : this(new[] { state }, tokenType, nodeBuilder)
        {
        }

        public TreeBuilderRule(IEnumerable<TreeBuilderState> states, TokenType tokenType, Func<Token, Queue<Token>, Node> nodeBuilder)
            : this(states, tokenType, nodeBuilder, null)
        {
        }

        public TreeBuilderRule(TreeBuilderState state, TokenType tokenType, Action<Stack<TreeBuilderState>> stateChange)
            : this(new[] { state }, tokenType, null, stateChange)
        {
        }

        internal TreeBuilderRule(
            IEnumerable<TreeBuilderState> states,
            TokenType tokenType,
            Func<Token, Queue<Token>, Node> nodeBuilder,
            Action<Stack<TreeBuilderState>> stateChange)
        {
            this.states = states;
            this.tokenType = tokenType;
            this.nodeBuilder = nodeBuilder;
            this.stateChange = stateChange;
        }

        public IEnumerable<TreeBuilderState> States
        {
            get { return states; }
        }

        public TokenType TokenType
        {
            get { return tokenType; }
        }

        public Func<Token, Queue<Token>, Node> NodeBuilder
        {
            get { return nodeBuilder; }
        }

        public Action<Stack<TreeBuilderState>> StateChange
        {
            get { return stateChange; }
        }
    }
}