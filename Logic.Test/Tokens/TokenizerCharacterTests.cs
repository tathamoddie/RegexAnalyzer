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
        public void Tokenizer_GetTokens_ShouldTokenizeAnyCharacterClass()
        {
            // Arrange
            const string input = ".";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.AnyCharacter, ".", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeBasicEscapeSequence()
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
        public void Tokenizer_GetTokens_ShouldTokenizeAsciiHexEscapeSequence()
        {
            // Arrange
            const string input = @"ab\x201";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Literal, "b", 1),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 2),
                    new Token(TokenType.CharacterEscapeHexMarker, "x", 3),
                    new Token(TokenType.CharacterEscapeData, "2", 4),
                    new Token(TokenType.CharacterEscapeData, "0", 5),
                    new Token(TokenType.Literal, "1", 6)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeEscapedPeriod()
        {
            // Arrange
            const string input = @"\.";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.CharacterEscapeMarker, @"\", 0),
                    new Token(TokenType.CharacterEscapeData, ".", 1)
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
                    new Token(TokenType.Character, "a", 1),
                    new Token(TokenType.Character, "b", 2),
                    new Token(TokenType.Character, "^", 3),
                    new Token(TokenType.Character, "c", 4),
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
                    new Token(TokenType.Character, "a", 2),
                    new Token(TokenType.Character, "b", 3),
                    new Token(TokenType.Character, "c", 4),
                    new Token(TokenType.CharacterSetEnd, "]", 5)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeAnyCharacterClassInCharacterSet()
        {
            // Arrange
            const string input = "[.ab]";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.CharacterSetStart, "[", 0),
                    new Token(TokenType.AnyCharacter, ".", 1),
                    new Token(TokenType.Character, "a", 2),
                    new Token(TokenType.Character, "b", 3),
                    new Token(TokenType.CharacterSetEnd, "]", 4)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeCharacterRange()
        {
            // Arrange
            const string input = "[a-z]";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.CharacterSetStart, "[", 0),
                    new Token(TokenType.Character, "a", 1),
                    new Token(TokenType.CharacterRangeSeparator, "-", 2),
                    new Token(TokenType.Character, "z", 3),
                    new Token(TokenType.CharacterSetEnd, "]", 4)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeCharacterRangeWithinOtherCharacters()
        {
            // Arrange
            const string input = "[1a-z2]";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.CharacterSetStart, "[", 0),
                    new Token(TokenType.Character, "1", 1),
                    new Token(TokenType.Character, "a", 2),
                    new Token(TokenType.CharacterRangeSeparator, "-", 3),
                    new Token(TokenType.Character, "z", 4),
                    new Token(TokenType.Character, "2", 5),
                    new Token(TokenType.CharacterSetEnd, "]", 6)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeDashAtStartOfCharacterSet()
        {
            // Arrange
            const string input = "[-ab]";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.CharacterSetStart, "[", 0),
                    new Token(TokenType.CharacterRangeSeparator, "-", 1),
                    new Token(TokenType.Character, "a", 2),
                    new Token(TokenType.Character, "b", 3),
                    new Token(TokenType.CharacterSetEnd, "]", 4)
                },
                result.ToArray()
            );
        }
    }
}