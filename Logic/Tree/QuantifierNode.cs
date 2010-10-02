using System.Linq;

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

        public override string Description
        {
            get
            {
                var commonDescriptions = new[]
                {
                    new { min = (int?)0, max = (int?)0, text = "never occurs" },
                    new { min = (int?)0, max = (int?)null, text = "zero or more occurences" },
                    new { min = (int?)0, max = (int?)1, text = "zero or one occurences" },
                    new { min = (int?)0, max = (int?)2, text = "up to two occurences" },
                    new { min = (int?)1, max = (int?)2, text = "one or two occurences" },
                    new { min = (int?)1, max = (int?)1, text = "exactly once" },
                    new { min = (int?)2, max = (int?)1, text = "exactly twice" },
                    new { min = (int?)1, max = (int?)null, text = "at least once" },
                    new { min = (int?)2, max = (int?)null, text = "at least twice" },
                    new { min = (int?)null, max = (int?)1, text = "at most once" },
                    new { min = (int?)null, max = (int?)2, text = "at most twice" },
                };

                var commonDescription = commonDescriptions
                    .Where(d => d.min == min)
                    .Where(d => d.max == max)
                    .SingleOrDefault();

                if (commonDescription != null)
                    return commonDescription.text;

                var format =
                    min.HasValue && max.HasValue && min == max ? "exactly {0:#,###} occurences" :
                    min == 0 && max.HasValue ? "up to {1:#,###} occurences" :
                    min.HasValue && max.HasValue ? "between {0:#,###} and {1:#,###} occurences" :
                    min.HasValue ? "at least {0:#,###} occurences" :
                    max.HasValue ? "at most {1:#,###} occurences" :
                    string.Empty;

                return string.Format(format, min ?? 0, max ?? 0);
            }
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