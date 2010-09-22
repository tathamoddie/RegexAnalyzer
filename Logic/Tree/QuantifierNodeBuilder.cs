using System.Collections.Generic;
using System.Linq;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    static class QuantifierNodeBuilder
    {
        public static Node BuildBasicQuantifierNode(Token startToken, TreeBuilderState state)
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

            return BuildQuantifierNode(new[] { startToken }, state, min, max);
        }

        public static Node BuildParametizedQuantifierNode(Token startToken, TreeBuilderState state)
        {
            var acceptedQuantifierTokenTypes = new[]
            {
                TokenType.Number,
                TokenType.ParametizedQuantifierRangeSeparator,
                TokenType.ParametizedQuantifierEnd
            };

            var remainingQuantifierTokens = state.ProcessingState.Tokens
                .DequeueWhile(t => acceptedQuantifierTokenTypes.Contains(t.Type));
            
            var quantifierTokens = new[] { startToken }.Concat(remainingQuantifierTokens);
            
            int? min;
            int? max;

            var quantifierTokenTypes = quantifierTokens.Select(t => t.Type);
            if (quantifierTokenTypes.SequenceEqual(new[]
                {
                    TokenType.ParametizedQuantifierStart,
                    TokenType.Number,
                    TokenType.ParametizedQuantifierRangeSeparator,
                    TokenType.Number,
                    TokenType.ParametizedQuantifierEnd
                }))
            {
                min = int.Parse(quantifierTokens.ElementAt(1).Data);
                max = int.Parse(quantifierTokens.ElementAt(3).Data);
            }
            else if (quantifierTokenTypes.SequenceEqual(new[]
                {
                    TokenType.ParametizedQuantifierStart,
                    TokenType.ParametizedQuantifierRangeSeparator,
                    TokenType.Number,
                    TokenType.ParametizedQuantifierEnd
                }))
            {
                min = null;
                max = int.Parse(quantifierTokens.ElementAt(2).Data);
            }
            else if (quantifierTokenTypes.SequenceEqual(new[]
                {
                    TokenType.ParametizedQuantifierStart,
                    TokenType.Number,
                    TokenType.ParametizedQuantifierRangeSeparator,
                    TokenType.ParametizedQuantifierEnd
                }))
            {
                min = int.Parse(quantifierTokens.ElementAt(1).Data);
                max = null;
            }
            else if (quantifierTokenTypes.SequenceEqual(new[]
                {
                    TokenType.ParametizedQuantifierStart,
                    TokenType.Number,
                    TokenType.ParametizedQuantifierEnd
                }))
            {
                min = int.Parse(quantifierTokens.ElementAt(1).Data);
                max = min;
            }
            else
            {
                return new LiteralNode(
                    Token.GetData(quantifierTokens),
                    startToken.StartIndex
                );
            }

            return BuildQuantifierNode(quantifierTokens, state, min, max);
        }

        static Node BuildQuantifierNode(IEnumerable<Token> quantifierTokens, TreeBuilderState state, int? min, int? max)
        {
            var targetNode = state.ProcessingState.TargetNode;
            var previousNodes = targetNode.Children;

            if (previousNodes.None())
                return new ParseFailureNode(quantifierTokens.First(), "Nothing preceeding the quantifier.");

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
                nodeToQuantify.Data + Token.GetData(quantifierTokens),
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