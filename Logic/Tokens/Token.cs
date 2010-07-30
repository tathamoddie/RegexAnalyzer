namespace TathamOddie.RegexAnalyzer.Logic.Tokens
{
    public class Token
    {
        readonly string data;
        readonly int startIndex;

        public Token(string data, int startIndex)
        {
            this.data = data;
            this.startIndex = startIndex;
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
            return typeof(Token).IsAssignableFrom(obj.GetType())
                && Equals((Token) obj);
        }

        public bool Equals(Token other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.data, data) && other.startIndex == startIndex;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((data != null ? data.GetHashCode() : 0)*397) ^ startIndex;
            }
        }
    }
}