using System.Collections.Generic;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class ProcessingState
    {
        readonly Node targetNode;
        readonly Queue<Token> tokens;

        public ProcessingState(Node targetNode, Queue<Token> tokens)
        {
            this.targetNode = targetNode;
            this.tokens = tokens;
        }

        public Node TargetNode
        {
            get { return targetNode; }
        }

        public Queue<Token> Tokens
        {
            get { return tokens; }
        }
    }
}