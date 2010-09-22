namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class GroupNode : Node
    {
        public GroupNode(string data, int startIndex)
            : base(data, startIndex)
        {}

        public GroupNode(string data, int startIndex, params Node[] children)
            : base(data, startIndex)
        {
            AddChildren(children);
        }
    }
}