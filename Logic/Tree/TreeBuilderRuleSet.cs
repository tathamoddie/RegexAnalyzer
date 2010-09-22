using System.Collections.Generic;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class TreeBuilderRuleSet : List<TreeBuilderRule>
    {
        public TreeBuilderRuleSet()
        {
            AddRange(BuildRules());
        }

        static IEnumerable<TreeBuilderRule> BuildRules()
        {
            // CharacterEscapeMarker starts a character escape
            yield return new TreeBuilderRule(
                TokenType.CharacterEscapeMarker,
                CharacterNodeBuilder.BuildCharacterNode
            );

            // GroupStart starts a group (duh!)
            yield return new TreeBuilderRule(
                TokenType.GroupStart,
                GroupNodeBuilder.BuildGroupNode
            );

            // Literal goes straight to Literal
            yield return new TreeBuilderRule(
                TokenType.Literal,
                (t, s) => new LiteralNode(t.Data, t.StartIndex)
            );

            // translate ParseFailure straight to a parse failure node
            yield return new TreeBuilderRule(
                TokenType.ParseFailure,
                (t, s) => new ParseFailureNode(t.Data, t.StartIndex, "Unrecognised token.")
            );
        }
    }
}