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
    }
}