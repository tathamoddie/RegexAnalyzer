using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class ContainerNode : Node
    {
        readonly IEnumerable<Node> children;

        public ContainerNode(string data, int startIndex, IEnumerable<Node> children)
            : base(data, startIndex)
        {
            this.children = children;
        }

        public IEnumerable<Node> Children
        {
            get { return children; }
        }
    }
}