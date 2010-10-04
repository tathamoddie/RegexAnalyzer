namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class ExpressionNode : Node
    {
        public ExpressionNode(string data, int startIndex)
            : base(data, startIndex)
        { }

        public ExpressionNode(string data, int startIndex, params Node[] children)
            : base(data, startIndex)
        {
            AddChildren(children);
        }

        public override string Description
        {
            get { return "expression"; }
        }
    }
}