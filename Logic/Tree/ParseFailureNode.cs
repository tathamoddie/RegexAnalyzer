using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class ParseFailureNode : Node
    {
        readonly string message;

        public ParseFailureNode(Token token, string message)
            : this(token.Data, token.StartIndex, message)
        {}

        public ParseFailureNode(string data, int startIndex, string message)
            : base(data, startIndex)
        {
            this.message = message;
        }

        public string Message
        {
            get { return message; }
        }

        public override string Description
        {
            get { return message; }
        }
    }
}