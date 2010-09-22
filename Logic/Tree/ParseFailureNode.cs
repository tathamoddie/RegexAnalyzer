namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class ParseFailureNode : Node
    {
        public ParseFailureNode(string data, int startIndex)
            : base(data, startIndex)
        {
        }
    }
}