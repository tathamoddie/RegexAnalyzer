using System;
using System.Collections.Generic;
using System.Linq;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    static class CharacterNodeBuilder
    {
        readonly static Dictionary<string, CharacterClass> CharacterClassMappings = new Dictionary<string, CharacterClass>
        {
            { "d", CharacterClass.Digits },
            { "D", CharacterClass.NonDigits },
            { "s", CharacterClass.WhiteSpace },
            { "S", CharacterClass.NonWhiteSpace },
            { "w", CharacterClass.Word },
            { "W", CharacterClass.NonWord },
        };

        public static Node BuildCharacterNode(Token startToken, TreeBuilderState state)
        {
            var dataTokenTypes = new[]
            {
                TokenType.CharacterEscapeData,
                TokenType.CharacterEscapeControlMarker,
                TokenType.CharacterEscapeHexMarker,
                TokenType.CharacterEscapeUnicodeMarker
            };

            var dataTokens = state
                .ProcessingState
                .Tokens
                .DequeueWhile(t => dataTokenTypes.Contains(t.Type));

            if (dataTokens.None())
                return new ParseFailureNode(startToken, "Character escape with no data.");

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