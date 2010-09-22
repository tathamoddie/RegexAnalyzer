using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class CharacterSetNode : Node
    {
        public CharacterSetNode(string data, int startIndex)
            : base(data, startIndex)
        {
        }
    }
}