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
            get { return "error: " + message; }
        }

        public override bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(((ParseFailureNode)other).message, message);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (message != null ? message.GetHashCode() : 0);
            }
        }
    }
}