using System.Collections.Generic;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class ProcessingState
    {
        readonly Queue<Token> tokens;

        public ProcessingState(Queue<Token> tokens)
        {
            this.tokens = tokens;
        }

        public Queue<Token> Tokens
        {
            get { return tokens; }
        }
    }
}