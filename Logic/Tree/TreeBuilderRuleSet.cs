using System.Collections.Generic;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class TreeBuilderRuleSet : List<TreeBuilderRule>
    {
        public TreeBuilderRuleSet()
        {
            AddRange(BuildCharacterEscapeRules());

            // translate ParseFailure straight to a parse failure node
            Add(new TreeBuilderRule(
                TreeBuilderRule.AnyState,
                TokenType.ParseFailure,
                (t, q) => new ParseFailureNode(t.Data, t.StartIndex)
            ));
        }

        static IEnumerable<TreeBuilderRule> BuildCharacterEscapeRules()
        {
            // CharacterEscapeMarker starts a character escape
            yield return new TreeBuilderRule(
                TreeBuilderState.Expression,
                TokenType.CharacterEscapeMarker,
                EscapedCharacterNode.Build
            );
        }
    }
}