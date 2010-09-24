namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class ExpressionNode : Node
    {
        public ExpressionNode(string data, int startIndex)
            : base(data, startIndex)
        { }

        public override string Description
        {
            get { return "expression"; }
        }
    }
}