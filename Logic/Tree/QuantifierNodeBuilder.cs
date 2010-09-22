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
                case "*": min = 0; max = null; break;
                case "+": min = 1; max = null; break;
                case "?": min = 0; max = 1; break;
                default:
                    return new ParseFailureNode(startToken, "Unrecognized quantifier sequence.");
            }

            var targetNode = state.ProcessingState.TargetNode;
            var previousNodes = targetNode.Children;

            if (previousNodes.None())
                return new ParseFailureNode(startToken, "Nothing preceeding the quantifier.");

            var immediatelyPriorNode = previousNodes.Last();

            // If there's a multi-character literal then we need to split it up
            Node nodeToQuantify;
            Node nodeToInsertBeforeQuantifier = null;
            if (immediatelyPriorNode is LiteralNode &&
                immediatelyPriorNode.Data.Length > 1)
            {
                var originalLiteralData = immediatelyPriorNode.Data;

                nodeToQuantify = new LiteralNode(
                    originalLiteralData.Substring(originalLiteralData.Length - 1),
                    immediatelyPriorNode.StartIndex + originalLiteralData.Length - 1);
                nodeToInsertBeforeQuantifier = new LiteralNode(
                    originalLiteralData.Substring(0, originalLiteralData.Length - 1),
                    immediatelyPriorNode.StartIndex);
            }
            else
            {
                nodeToQuantify = immediatelyPriorNode;
            }

            var quantifierNode = new QuantifierNode(
                nodeToQuantify.Data + startToken.Data,
                nodeToQuantify.StartIndex,
                min,
                max,
                nodeToQuantify
            );

            targetNode.ReplaceLastChild(nodeToInsertBeforeQuantifier, quantifierNode);

            return null;
        }
    }
}