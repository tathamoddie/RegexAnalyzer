using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tokens
{
    [TestClass]
    public class TokenizerQuantifierTests
    {
        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeZeroOrMoreQuantifier()
        {
            // Arrange
            const string input = "a*";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "*", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeZeroOrMoreLazyQuantifier()
        {
            // Arrange
            const string input = "a*?";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "*", 1),
                    new Token(TokenType.Quantifier, "?", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeZeroOrOneQuantifier()
        {
            // Arrange
            const string input = "a?";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "?", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeZeroOrOneQuantifierInGroup()
        {
            // Arrange
            const string input = "(a?)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.Literal, "a", 1),
                    new Token(TokenType.Quantifier, "?", 2),
                    new Token(TokenType.GroupEnd, ")", 3)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOneOrMoreQuantifier()
        {
            // Arrange
            const string input = "a+";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "+", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOneOrMoreLazyQuantifier()
        {
            // Arrange
            const string input = "a+?";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "+", 1),
                    new Token(TokenType.Quantifier, "?", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeParametizedQuantifier()
        {
            // Arrange
            const string input = "a{6}";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.ParametizedQuantifierStart, "{", 1),
                    new Token(TokenType.Number, "6", 2),
                    new Token(TokenType.ParametizedQuantifierEnd, "}", 3)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeParametizedMinimumQuantifier()
        {
            // Arrange
            const string input = "a{6,}";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.ParametizedQuantifierStart, "{", 1),
                    new Token(TokenType.Number, "6", 2),
                    new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 3),
                    new Token(TokenType.ParametizedQuantifierEnd, "}", 4)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeParametizedRangeQuantifier()
        {
            // Arrange
            const string input = "a{6,24}";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.ParametizedQuantifierStart, "{", 1),
                    new Token(TokenType.Number, "6", 2),
                    new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 3),
                    new Token(TokenType.Number, "2", 4),
                    new Token(TokenType.Number, "4", 5),
                    new Token(TokenType.ParametizedQuantifierEnd, "}", 6)
                },
                result.ToArray()
            );
        }
    }
}