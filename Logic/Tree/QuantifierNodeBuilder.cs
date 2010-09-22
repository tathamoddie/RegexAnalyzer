using System.Linq;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    static class QuantifierNodeBuilder
    {
        public static Node BuildQuantifierNode(Token startToken, TreeBuilderState state)
        {
            int? min;
            int? max;

            switch (startToken.Data)
            {
                case "?":
                    min = 0;
                    max = 1;
                    break;
                default:
                    return new ParseFailureNode(startToken, "Unrecognized quantifier sequence.");
            }

            var targetNode = state.ProcessingState.TargetNode;
            var previousNodes = targetNode.Children;

            if (previousNodes.None())
                return new ParseFailureNode(startToken, "Nothing preceeding the quantifier.");

            var immediatelyPriorNode = previousNodes.Last();

            var quantifierNode = new QuantifierNode(
                immediatelyPriorNode.Data + startToken.Data,
                immediatelyPriorNode.StartIndex,
                min,
                max,
                immediatelyPriorNode
            );

            targetNode.ReplaceLastChild(quantifierNode);

            return null;
        }
    }
}