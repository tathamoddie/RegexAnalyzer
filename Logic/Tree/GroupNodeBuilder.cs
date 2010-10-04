using System;
using System.Collections.Generic;
using System.Linq;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    static class GroupNodeBuilder
    {
        public static Node BuildGroupNode(Token startToken, TreeBuilderState state)
        {
            var contentsTokens = new List<Token>();
            Token endToken = null;

            var nestedGroupDepth = 0;
            while (state.ProcessingState.Tokens.Any())
            {
                var token = state.ProcessingState.Tokens.Dequeue();

                switch (token.Type)
                {
                    case TokenType.GroupStart:
                        nestedGroupDepth++;
                        break;
                    case TokenType.GroupEnd:
                        nestedGroupDepth--;
                        break;
                }

                if (nestedGroupDepth >= 0)
                {
                    contentsTokens.Add(token);
                }
                else
                {
                    endToken = token;
                    break;
                }
            }

            if (endToken == null)
                return new ParseFailureNode(startToken, "Group is never closed.");

            var combinedData = Token.GetData(startToken, contentsTokens, endToken);

            var groupNode = new GroupNode(
                combinedData,
                startToken.StartIndex
            );

            // Queue the group contents for processing
            state.ProcessingQueue.Enqueue(new KeyValuePair<Node, IEnumerable<Token>>(
                groupNode,
                contentsTokens
            ));

            return groupNode;
        }

        public static Node ProcessGroupDirective(Token startToken, TreeBuilderState state)
        {
            var group = (GroupNode)state.ProcessingState.TargetNode;

            var strategies = new Dictionary<IEnumerable<PatternSegment<Token>>, Action<GroupNode, IEnumerable<Token>>>
            {
                // Named group
                {
                    new[]
                    {
                        new PatternSegment<Token>(t => t.Type == TokenType.NamedIdentifierStartOrLookBehindMarker, 1),
                        new PatternSegment<Token>(t => t.Type == TokenType.Literal),
                        new PatternSegment<Token>(t => t.Type == TokenType.NamedIdentifierEnd, 1),
                    },
                    ProcessNamedGroupDirective
                },

                // Balancing group
                {
                    new[]
                    {
                        new PatternSegment<Token>(t => t.Type == TokenType.NamedIdentifierStartOrLookBehindMarker, 1),
                        new PatternSegment<Token>(t => t.Type == TokenType.Literal),
                        new PatternSegment<Token>(t => t.Type == TokenType.BalancingGroupNamedIdentifierSeparator, 1),
                        new PatternSegment<Token>(t => t.Type == TokenType.Literal),
                        new PatternSegment<Token>(t => t.Type == TokenType.NamedIdentifierEnd, 1),
                    },
                    ProcessBalancingGroupDirective
                },

                // Non-capturing
                {
                    new[]
                    {
                        new PatternSegment<Token>(t => t.Type == TokenType.NonCapturingGroupMarker, 1)
                    },
                    (g, t) => { g.IsCapturing = false; }
                },
            };

            foreach (var strategy in strategies)
            {
                var tokens = state
                    .ProcessingState
                    .Tokens
                    .DequeuePattern(strategy.Key);

                if (!tokens.Any()) continue;

                strategy.Value(group, tokens);

                break;
            }

            return null;
        }

        static void ProcessNamedGroupDirective(GroupNode group, IEnumerable<Token> tokens)
        {
            var identifierTokens = tokens
                .Skip(1)
                .Take(tokens.Count() - 2);

            var identifier = Token.GetData(identifierTokens);

            group.NamedIdentifier = identifier;
        }

        static void ProcessBalancingGroupDirective(GroupNode group, IEnumerable<Token> tokens)
        {
            var identifierTokens = tokens
                .Skip(1)
                .TakeWhile(t => t.Type == TokenType.Literal);

            var namedIdentifier = Token.GetData(identifierTokens);

            group.NamedIdentifier = namedIdentifier;

            var balancingGroupTokens = tokens
                .Skip(1) // ?
                .Skip(identifierTokens.Count())
                .Skip(1) // -
                .TakeWhile(t => t.Type == TokenType.Literal);

            var balancingGroupIdentifier = Token.GetData(balancingGroupTokens);

            group.BalancingGroupIdentifier = balancingGroupIdentifier;
        }
    }
}