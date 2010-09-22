using System;
using System.Collections.Generic;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class TreeBuilderRule
    {
        public const IEnumerable<TreeBuilderState> AnyState = null;

        readonly TokenType tokenType;
        readonly Func<Token, TreeBuilderState, Node> nodeBuilder;

        public TreeBuilderRule(TokenType tokenType, Func<Token, TreeBuilderState, Node> nodeBuilder)
        {
            this.tokenType = tokenType;
            this.nodeBuilder = nodeBuilder;
        }

        public TokenType TokenType
        {
            get { return tokenType; }
        }

        public Func<Token, TreeBuilderState, Node> NodeBuilder
        {
            get { return nodeBuilder; }
        }
    }
}