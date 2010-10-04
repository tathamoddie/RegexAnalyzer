namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class GroupNode : Node
    {
        public GroupNode(string data, int startIndex)
            : base(data, startIndex)
        {}

        public GroupNode(string data, int startIndex, params Node[] children)
            : base(data, startIndex)
        {
            AddChildren(children);
        }

        public override string Description
        {
            get
            {
                return
                    !string.IsNullOrEmpty(BalancingGroupIdentifier) ? string.Format("balancing group '{0}', captured in '{1}'", BalancingGroupIdentifier, NamedIdentifier) :
                    !string.IsNullOrEmpty(NamedIdentifier) ? string.Format("named group '{0}'", NamedIdentifier)
                    : "group";
            }
        }

        public string NamedIdentifier { get; set; }

        public string BalancingGroupIdentifier { get; set; }

        bool isCapturing = true;
        public bool IsCapturing
        {
            get { return isCapturing; }
            set { isCapturing = value; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as GroupNode);
        }

        public bool Equals(GroupNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other)
                && other.isCapturing.Equals(isCapturing)
                && Equals(other.NamedIdentifier, NamedIdentifier)
                && Equals(other.BalancingGroupIdentifier, BalancingGroupIdentifier);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ isCapturing.GetHashCode();
                result = (result*397) ^ (NamedIdentifier != null ? NamedIdentifier.GetHashCode() : 0);
                result = (result*397) ^ (BalancingGroupIdentifier != null ? BalancingGroupIdentifier.GetHashCode() : 0);
                return result;
            }
        }
    }
}