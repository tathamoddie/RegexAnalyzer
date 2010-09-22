namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class EscapedCharacterNode : Node
    {
        readonly string escapedContent;

        public EscapedCharacterNode(string data, int startIndex, string escapedContent)
            : base(data, startIndex)
        {
            this.escapedContent = escapedContent;
        }

        public string EscapedContent
        {
            get { return escapedContent; }
        }
    }
}