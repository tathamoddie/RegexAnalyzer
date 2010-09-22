using System.Collections.Generic;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class TreeBuilderState
    {
        readonly Queue<KeyValuePair<Node, IEnumerable<Token>>> processingQueue = new Queue<KeyValuePair<Node, IEnumerable<Token>>>();

        public Queue<KeyValuePair<Node, IEnumerable<Token>>> ProcessingQueue
        {
            get { return processingQueue; }
        }

        public ProcessingState ProcessingState { get; set; }
    }
}