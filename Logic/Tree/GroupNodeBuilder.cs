﻿using System.Collections.Generic;
using System.Linq;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    static class GroupNodeBuilder
    {
        public static Node BuildGroupNode(Token startToken, Queue<Token> tokens)
        {
            var contentsTokens = new List<Token>();
            Token endToken = null;

            var nestedGroupDepth = 0;
            while (tokens.Any())
            {
                var token = tokens.Dequeue();

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

            return new GroupNode(
                combinedData,
                startToken.StartIndex
            );
        }
    }
}