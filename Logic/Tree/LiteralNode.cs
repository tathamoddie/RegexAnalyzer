namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class LiteralNode : Node
    {
        readonly string unescapedContent;

        public LiteralNode(string data, int startIndex, string unescapedContent)
            : base(data, startIndex)
        {
            this.unescapedContent = unescapedContent;
        }

        public string UnescapedContent
        {
            get { return unescapedContent; }
        }
    }
}