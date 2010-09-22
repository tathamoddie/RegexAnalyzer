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
        readonly Func<Token, Node> nodeBuilder;

        public TreeBuilderRule(TreeBuilderState state, TokenType tokenType, Func<Token, Node> nodeBuilder)
            : this(new[] { state }, tokenType, nodeBuilder)
        {
        }

        public TreeBuilderRule(IEnumerable<TreeBuilderState> states, TokenType tokenType, Func<Token, Node> nodeBuilder)
        {
            this.states = states;
            this.tokenType = tokenType;
            this.nodeBuilder = nodeBuilder;
        }

        public IEnumerable<TreeBuilderState> States
        {
            get { return states; }
        }

        public TokenType TokenType
        {
            get { return tokenType; }
        }

        public Func<Token, Node> NodeBuilder
        {
            get { return nodeBuilder; }
        }
    }
}