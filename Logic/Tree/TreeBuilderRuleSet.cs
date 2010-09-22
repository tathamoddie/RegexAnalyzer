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
            AddRange(BuildCharacterEscapeRules());

            // translate ParseFailure straight to a parse failure node
            Add(new TreeBuilderRule(
                TreeBuilderRule.AnyState,
                TokenType.ParseFailure,
                (t, q) => new ParseFailureNode(t.Data, t.StartIndex)
            ));
        }

        static IEnumerable<TreeBuilderRule> BuildCharacterEscapeRules()
        {
            // CharacterEscapeMarker starts a character escape
            yield return new TreeBuilderRule(
                TreeBuilderState.Expression,
                TokenType.CharacterEscapeMarker,
                BuildCharacterEscapeNode
            );
        }

        readonly static Dictionary<string, CharacterClass> CharacterClassMappings = new Dictionary<string, CharacterClass>
        {
            { "d", CharacterClass.Digits },
            { "D", CharacterClass.NonDigits },
            { "s", CharacterClass.WhiteSpace },
            { "S", CharacterClass.NonWhiteSpace },
            { "w", CharacterClass.Word },
            { "W", CharacterClass.NonWord },
        };

        static Node BuildCharacterEscapeNode(Token startToken, Queue<Token> tokens)
        {
            var dataTokenTypes = new[]
            {
                TokenType.CharacterEscapeData,
                TokenType.CharacterEscapeControlMarker,
                TokenType.CharacterEscapeHexMarker,
                TokenType.CharacterEscapeUnicodeMarker
            };

            var dataTokens = tokens
                .DequeueWhile(t => dataTokenTypes.Contains(t.Type));

            if (dataTokens.None())
                return new ParseFailureNode(startToken);

            if (dataTokens.Any(t => t.Type != TokenType.CharacterEscapeData))
                throw new NotImplementedException();

            var escapedContent = dataTokens
                .Aggregate(string.Empty, (d, t) => d + t.Data);

            var combinedData = Token.GetData(startToken, dataTokens);

            if (CharacterClassMappings.ContainsKey(escapedContent))
            {
                var characterClass = CharacterClassMappings[escapedContent];
                return new CharacterClassNode(combinedData, startToken.StartIndex, characterClass);
            }

            return new EscapedCharacterNode(
                combinedData,
                startToken.StartIndex,
                escapedContent
            );
        }
    }
}