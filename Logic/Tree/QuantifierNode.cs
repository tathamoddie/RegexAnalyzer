namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class QuantifierNode : Node
    {
        readonly int? min;
        readonly int? max;

        public QuantifierNode(string data, int startIndex, int? min, int? max, Node child)
            : base(data, startIndex)
        {
            this.min = min;
            this.max = max;

            AddChild(child);
        }

        public int? Max
        {
            get { return max; }
        }

        public int? Min
        {
            get { return min; }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj)
                && ((QuantifierNode)obj).Min == min
                && ((QuantifierNode)obj).Max == max;
        }

        public bool Equals(QuantifierNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.min.Equals(min) && other.max.Equals(max);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ (min.HasValue ? min.Value : 0);
                result = (result*397) ^ (max.HasValue ? max.Value : 0);
                return result;
            }
        }
    }
}