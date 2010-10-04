using System;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class LiteralNode : Node
    {
        public LiteralNode(string data, int startIndex)
            : base(data, startIndex)
        { }

        public override string Description
        {
            get { return "literal text"; }
        }
    }
}