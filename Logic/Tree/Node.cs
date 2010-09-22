namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public abstract class Node
    {
        readonly string data;
        readonly int startIndex;

        protected Node(string data, int startIndex)
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
            if (obj.GetType() != GetType()) return false;
            return Equals((Node) obj);
        }

        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.data, data) && other.startIndex == startIndex;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (data != null ? data.GetHashCode() : 0);
                result = (result*397) ^ startIndex;
                return result;
            }
        }
    }
}