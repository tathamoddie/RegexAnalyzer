using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class ParseFailureNode : Node
    {
        public ParseFailureNode(Token token)
            : this(token.Data, token.StartIndex)
        {}

        public ParseFailureNode(string data, int startIndex)
            : base(data, startIndex)
        {}
    }
}