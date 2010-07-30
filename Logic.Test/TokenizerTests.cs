using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TathamOddie.RegexAnalyzer.Logic.Test
{
    [TestClass]
    public class TokenizerTests
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
        public void Tokenizer_GetTokens_ShouldTokenizeGroupDirectiveStart()
        {
            // Arrange
            const string input = "(?";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldReturnParseFailureWhenClosingGroupImmediatelyAfterGroupDirectiveStartToken()
        {
            // Arrange
            const string input = "(?)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ParseFailure, ")", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeGroup()
        {
            // Arrange
            const string input = "(foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.Literal, "f", 1),
                    new Token(TokenType.Literal, "o", 2),
                    new Token(TokenType.Literal, "o", 3),
                    new Token(TokenType.GroupEnd, ")", 4)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNonCapturingGroup()
        {
            // Arrange
            const string input = "(?:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.NonCapturingGroupMarker, ":", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.GroupEnd, ")", 6)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNamedGroup()
        {
            // Arrange
            const string input = "(?<foo>bar)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.NamedIdentifierStart, "<", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.NamedIdentifierEnd, ">", 6),
                    new Token(TokenType.Literal, "b", 7),
                    new Token(TokenType.Literal, "a", 8),
                    new Token(TokenType.Literal, "r", 9),
                    new Token(TokenType.GroupEnd, ")", 10)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOption()
        {
            // Arrange
            const string input = "(?m:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOption, "m", 2),
                    new Token(TokenType.GroupOptionEnd, ":", 3),
                    new Token(TokenType.Literal, "f", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.GroupEnd, ")", 7)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeMultipleOptions()
        {
            // Arrange
            const string input = "(?mis:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOption, "m", 2),
                    new Token(TokenType.GroupOption, "i", 3),
                    new Token(TokenType.GroupOption, "s", 4),
                    new Token(TokenType.GroupOptionEnd, ":", 5),
                    new Token(TokenType.Literal, "f", 6),
                    new Token(TokenType.Literal, "o", 7),
                    new Token(TokenType.Literal, "o", 8),
                    new Token(TokenType.GroupEnd, ")", 9)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNestedOptions()
        {
            // Arrange
            const string input = "(?m:(?i:(?s:foo)))";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOption, "m", 2),
                    new Token(TokenType.GroupOptionEnd, ":", 3),
                    new Token(TokenType.GroupStart, "(", 4),
                    new Token(TokenType.GroupDirectiveStart, "?", 5),
                    new Token(TokenType.GroupOption, "i", 6),
                    new Token(TokenType.GroupOptionEnd, ":", 7),
                    new Token(TokenType.GroupStart, "(", 8),
                    new Token(TokenType.GroupDirectiveStart, "?", 9),
                    new Token(TokenType.GroupOption, "s", 10),
                    new Token(TokenType.GroupOptionEnd, ":", 11),
                    new Token(TokenType.Literal, "f", 12),
                    new Token(TokenType.Literal, "o", 13),
                    new Token(TokenType.Literal, "o", 14),
                    new Token(TokenType.GroupEnd, ")", 15),
                    new Token(TokenType.GroupEnd, ")", 16),
                    new Token(TokenType.GroupEnd, ")", 17)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNegativeQualifiedOption()
        {
            // Arrange
            const string input = "(?-m:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOptionQualifier, "-", 2),
                    new Token(TokenType.GroupOption, "m", 3),
                    new Token(TokenType.GroupOptionEnd, ":", 4),
                    new Token(TokenType.Literal, "f", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.Literal, "o", 7),
                    new Token(TokenType.GroupEnd, ")", 8)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizePositiveQualifiedOption()
        {
            // Arrange
            const string input = "(?+m:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOptionQualifier, "+", 2),
                    new Token(TokenType.GroupOption, "m", 3),
                    new Token(TokenType.GroupOptionEnd, ":", 4),
                    new Token(TokenType.Literal, "f", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.Literal, "o", 7),
                    new Token(TokenType.GroupEnd, ")", 8)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeMultipleQualifiedOptions()
        {
            // Arrange
            const string input = "(?+m-si:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOptionQualifier, "+", 2),
                    new Token(TokenType.GroupOption, "m", 3),
                    new Token(TokenType.GroupOptionQualifier, "-", 4),
                    new Token(TokenType.GroupOption, "s", 5),
                    new Token(TokenType.GroupOption, "i", 6),
                    new Token(TokenType.GroupOptionEnd, ":", 7),
                    new Token(TokenType.Literal, "f", 8),
                    new Token(TokenType.Literal, "o", 9),
                    new Token(TokenType.Literal, "o", 10),
                    new Token(TokenType.GroupEnd, ")", 11)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleLiteralTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.Literal, "b", 1),
                new Token(TokenType.Literal, "c", 2)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "abc", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleLiteralTokensWithinOtherTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.GroupStart, "(", 1),
                new Token(TokenType.Literal, "b", 2),
                new Token(TokenType.Literal, "c", 3),
                new Token(TokenType.Literal, "d", 4),
                new Token(TokenType.GroupEnd, ")", 5),
                new Token(TokenType.Literal, "e", 6)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.GroupStart, "(", 1),
                    new Token(TokenType.Literal, "bcd", 2),
                    new Token(TokenType.GroupEnd, ")", 5),
                    new Token(TokenType.Literal, "e", 6)
                },
                result.ToArray()
            );
        }
    }
}