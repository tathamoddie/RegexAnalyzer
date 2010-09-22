using System.Collections.Generic;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class TreeBuilderRuleSet : List<TreeBuilderRule>
    {
        public TreeBuilderRuleSet()
        {
            Add(new TreeBuilderRule(
                TreeBuilderRule.AnyState,
                TokenType.ParseFailure,
                t => new ParseFailureNode(t.Data, t.StartIndex)
            ));
        }
    }
}
