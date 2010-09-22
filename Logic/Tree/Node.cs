using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public abstract class Node
    {
        readonly IList<Node> children = new List<Node>();
        readonly string data;
        readonly int startIndex;

        protected Node(string data, int startIndex)
        {
            this.data = data;
            this.startIndex = startIndex;
        }

        public IEnumerable<Node> Children
        {
            get { return new ReadOnlyCollection<Node>(children); }
        }

        public string Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }
    }
}