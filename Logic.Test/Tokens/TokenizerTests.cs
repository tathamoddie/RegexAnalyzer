using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tokens
{
    [TestClass]
    public class TokenizerTests
    {
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
        public void Tokenizer_GetTokens_ShouldTokenizeOrOperator()
        {
            // Arrange
            const string input = "cat|dog|tiger";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "c", 0),
                    new Token(TokenType.Literal, "a", 1),
                    new Token(TokenType.Literal, "t", 2),
                    new Token(TokenType.OrOperator, "|", 3),
                    new Token(TokenType.Literal, "d", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.Literal, "g", 6),
                    new Token(TokenType.OrOperator, "|", 7),
                    new Token(TokenType.Literal, "t", 8),
                    new Token(TokenType.Literal, "i", 9),
                    new Token(TokenType.Literal, "g", 10),
                    new Token(TokenType.Literal, "e", 11),
                    new Token(TokenType.Literal, "r", 12)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOrOperatorInGroup()
        {
            // Arrange
            const string input = "(cat|dog|tiger)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.Literal, "c", 1),
                    new Token(TokenType.Literal, "a", 2),
                    new Token(TokenType.Literal, "t", 3),
                    new Token(TokenType.OrOperator, "|", 4),
                    new Token(TokenType.Literal, "d", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.Literal, "g", 7),
                    new Token(TokenType.OrOperator, "|", 8),
                    new Token(TokenType.Literal, "t", 9),
                    new Token(TokenType.Literal, "i", 10),
                    new Token(TokenType.Literal, "g", 11),
                    new Token(TokenType.Literal, "e", 12),
                    new Token(TokenType.Literal, "r", 13),
                    new Token(TokenType.GroupEnd, ")", 14)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeStartOfStringAssertion()
        {
            // Arrange
            const string input = "^a";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.StartOfStringAssertion, "^", 0),
                    new Token(TokenType.Literal, "a", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeEndOfStringAssertion()
        {
            // Arrange
            const string input = "a$";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.EndOfStringAssertion, "$", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleConsecutiveLiteralTokens()
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
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleConsecutiveQuantifierTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.Quantifier, "a", 0),
                new Token(TokenType.Quantifier, "b", 1),
                new Token(TokenType.Quantifier, "c", 2)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Quantifier, "abc", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleConsecutiveGroupOptionTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.GroupOption, "a", 0),
                new Token(TokenType.GroupOption, "b", 1),
                new Token(TokenType.GroupOption, "c", 2)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupOption, "abc", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleConsecutiveNumberTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.Number, "1", 0),
                new Token(TokenType.Number, "2", 1),
                new Token(TokenType.Number, "3", 2)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Number, "123", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldNotCombineMultipleConsecutiveGroupEndTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.GroupEnd, "a", 0),
                new Token(TokenType.GroupEnd, "b", 1),
                new Token(TokenType.GroupEnd, "c", 2)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupEnd, "a", 0),
                    new Token(TokenType.GroupEnd, "b", 1),
                    new Token(TokenType.GroupEnd, "c", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleConsecutiveLiteralAndQuantifierTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.GroupStart, "(", 1),
                new Token(TokenType.GroupStart, "(", 2),
                new Token(TokenType.Literal, "b", 3),
                new Token(TokenType.Literal, "c", 4),
                new Token(TokenType.Quantifier, "*", 5),
                new Token(TokenType.Quantifier, "?", 6),
                new Token(TokenType.Literal, "d", 7),
                new Token(TokenType.GroupEnd, ")", 8),
                new Token(TokenType.GroupEnd, ")", 9),
                new Token(TokenType.Literal, "e", 10)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.GroupStart, "(", 1),
                    new Token(TokenType.GroupStart, "(", 2),
                    new Token(TokenType.Literal, "bc", 3),
                    new Token(TokenType.Quantifier, "*?", 5),
                    new Token(TokenType.Literal, "d", 7),
                    new Token(TokenType.GroupEnd, ")", 8),
                    new Token(TokenType.GroupEnd, ")", 9),
                    new Token(TokenType.Literal, "e", 10)
                },
                result.ToArray()
            );
        }
    }
}