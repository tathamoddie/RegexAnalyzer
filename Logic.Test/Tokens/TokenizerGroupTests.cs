using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tokens
{
    [TestClass]
    public class TokenizerGroupTests
    {
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
                    new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 2),
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
        public void Tokenizer_GetTokens_ShouldTokenizeInlineComment()
        {
            // Arrange
            const string input = "(?#foo!)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.CommentStart, "#", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.Literal, "!", 6),
                    new Token(TokenType.GroupEnd, ")", 7)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizePositiveLookAhead()
        {
            // Arrange
            const string input = "(?=foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.PositiveLookAheadMarker, "=", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.GroupEnd, ")", 6)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNegativeLookAhead()
        {
            // Arrange
            const string input = "(?!foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.NegativeLookAheadMarker, "!", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.GroupEnd, ")", 6)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizePositiveLookBehind()
        {
            // Arrange
            const string input = "(?<=foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 2),
                    new Token(TokenType.PositiveLookBehindMarker, "=", 3),
                    new Token(TokenType.Literal, "f", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.GroupEnd, ")", 7)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNegativeLookBehind()
        {
            // Arrange
            const string input = "(?<!foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 2),
                    new Token(TokenType.NegativeLookBehindMarker, "!", 3),
                    new Token(TokenType.Literal, "f", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.GroupEnd, ")", 7)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNonBacktrackingSubexpression()
        {
            // Arrange
            const string input = "(?>foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.NonBacktrackingSubExpressionMarker, ">", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.GroupEnd, ")", 6)
                },
                result.ToArray()
            );
        }
    }
}