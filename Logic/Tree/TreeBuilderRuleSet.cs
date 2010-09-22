using System;
using System.Collections.Generic;
using System.Linq;
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
                TreeBuilderState.Expression,
                TokenType.CharacterEscapeMarker,
                CharacterNodeBuilder.BuildCharacterNode
            );

            // GroupStart starts a group (duh!)
            yield return new TreeBuilderRule(
                TreeBuilderState.Expression,
                TokenType.GroupStart,
                GroupNodeBuilder.BuildGroupNode
            );

            // translate ParseFailure straight to a parse failure node
            yield return new TreeBuilderRule(
                TreeBuilderRule.AnyState,
                TokenType.ParseFailure,
                (t, q) => new ParseFailureNode(t.Data, t.StartIndex, "Unrecognised token.")
            );
        }
    }
}