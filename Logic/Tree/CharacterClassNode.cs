using System;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class CharacterClassNode : Node
    {
        readonly CharacterClass characterClass;

        public CharacterClassNode(string data, int startIndex, CharacterClass characterClass)
            : base(data, startIndex)
        {
            this.characterClass = characterClass;
        }

        public CharacterClass CharacterClass
        {
            get { return characterClass; }
        }

        public override string Description
        {
            get { throw new NotImplementedException(); }
        }
    }
}