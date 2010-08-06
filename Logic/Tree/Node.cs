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
    }
}