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

        public override string Description
        {
            get
            {
                return !string.IsNullOrEmpty(NamedIdentifier)
                    ? string.Format("named group '{0}'", NamedIdentifier)
                    : "group";
            }
        }

        public string NamedIdentifier { get; set; }
    }
}