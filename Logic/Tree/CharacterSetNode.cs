using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class CharacterSetNode : ContainerNode
    {
        public CharacterSetNode(string data, int startIndex, IEnumerable<Node> children)
            : base(data, startIndex, children)
        {
        }
    }
}