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
            yield return new TreeBuilderRule(
                TokenType.CharacterEscapeMarker,
                CharacterNodeBuilder.BuildCharacterNode
            );

            yield return new TreeBuilderRule(
                TokenType.GroupStart,
                GroupNodeBuilder.BuildGroupNode
            );

            yield return new TreeBuilderRule(
                TokenType.Quantifier,
                QuantifierNodeBuilder.BuildBasicQuantifierNode
            );

            yield return new TreeBuilderRule(
                TokenType.ParametizedQuantifierStart,
                QuantifierNodeBuilder.BuildParametizedQuantifierNode
            );

            yield return new TreeBuilderRule(
                TokenType.Literal,
                (t, s) => new LiteralNode(t.Data, t.StartIndex)
            );

            yield return new TreeBuilderRule(
                TokenType.ParseFailure,
                (t, s) => new ParseFailureNode(t.Data, t.StartIndex, "unrecognised token")
            );
        }
    }
}