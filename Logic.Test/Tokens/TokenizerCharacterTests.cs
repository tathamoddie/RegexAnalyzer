using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tokens
{
    [TestClass]
    public class TokenizerCharacterTests
    {
        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeLiteralCharacter()
        {
            // Arrange
            const string input = "a";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeMultipleLiteralCharacters()
        {
            // Arrange
            const string input = "abc";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Literal, "b", 1),
                    new Token(TokenType.Literal, "c", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeEscapeSequence()
        {
            // Arrange
            const string input = @"ab\+c";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Literal, "b", 1),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 2),
                    new Token(TokenType.CharacterEscapeData, "+", 3),
                    new Token(TokenType.Literal, "c", 4)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeCharacterSet()
        {
            // Arrange
            const string input = "[ab^c]";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.CharacterSetStart, "[", 0),
                    new Token(TokenType.Literal, "a", 1),
                    new Token(TokenType.Literal, "b", 2),
                    new Token(TokenType.Literal, "^", 3),
                    new Token(TokenType.Literal, "c", 4),
                    new Token(TokenType.CharacterSetEnd, "]", 5)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNegativeCharacterSet()
        {
            // Arrange
            const string input = "[^abc]";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.CharacterSetStart, "[", 0),
                    new Token(TokenType.NegativeCharacterSetModifier, "^", 1),
                    new Token(TokenType.Literal, "a", 2),
                    new Token(TokenType.Literal, "b", 3),
                    new Token(TokenType.Literal, "c", 4),
                    new Token(TokenType.CharacterSetEnd, "]", 5)
                },
                result.ToArray()
            );
        }
    }
}