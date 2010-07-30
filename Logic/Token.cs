using System.Diagnostics;

namespace TathamOddie.RegexAnalyzer.Logic
{
    [DebuggerDisplay("{Type} @ {StartIndex}: {Data}")]
    public class Token
    {
        readonly TokenType type;
        readonly string data;
        readonly int startIndex;

        public Token(TokenType type, string data, int startIndex)
        {
            this.type = type;
            this.data = data;
            this.startIndex = startIndex;
        }

        public TokenType Type
        {
            get { return type; }
        }

        public string Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Token)) return false;
            return Equals((Token) obj);
        }

        public bool Equals(Token other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.type, type) && Equals(other.data, data) && other.startIndex == startIndex;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = type.GetHashCode();
                result = (result*397) ^ (data != null ? data.GetHashCode() : 0);
                result = (result*397) ^ startIndex;
                return result;
            }
        }
    }
}