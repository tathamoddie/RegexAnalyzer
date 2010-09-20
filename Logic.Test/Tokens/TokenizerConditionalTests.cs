using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tokens
{
    [TestClass]
    public class TokenizerConditionalTests
    {
        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOnePartConditional()
        {
            // Arrange
            const string input = "(?(foo)bar)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ConditionalExpressionStart, "(", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 6),
                    new Token(TokenType.Literal, "b", 7),
                    new Token(TokenType.Literal, "a", 8),
                    new Token(TokenType.Literal, "r", 9),
                    new Token(TokenType.GroupEnd, ")", 10)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOnePartConditionalWithNestedNamedGroup()
        {
            // Arrange
            const string input = "(?(foo)(?<bar>baz))";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ConditionalExpressionStart, "(", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 6),
                    new Token(TokenType.GroupStart, "(", 7),
                    new Token(TokenType.GroupDirectiveStart, "?", 8),
                    new Token(TokenType.NamedIdentifierStart, "<", 9),
                    new Token(TokenType.Literal, "b", 10),
                    new Token(TokenType.Literal, "a", 11),
                    new Token(TokenType.Literal, "r", 12),
                    new Token(TokenType.NamedIdentifierEnd, ">", 13),
                    new Token(TokenType.Literal, "b", 14),
                    new Token(TokenType.Literal, "a", 15),
                    new Token(TokenType.Literal, "z", 16),
                    new Token(TokenType.GroupEnd, ")", 17),
                    new Token(TokenType.GroupEnd, ")", 18)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeTwoPartConditional()
        {
            // Arrange
            const string input = "(?(foo)bar|baz)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ConditionalExpressionStart, "(", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 6),
                    new Token(TokenType.Literal, "b", 7),
                    new Token(TokenType.Literal, "a", 8),
                    new Token(TokenType.Literal, "r", 9),
                    new Token(TokenType.OrOperator, "|", 10),
                    new Token(TokenType.Literal, "b", 11),
                    new Token(TokenType.Literal, "a", 12),
                    new Token(TokenType.Literal, "z", 13),
                    new Token(TokenType.GroupEnd, ")", 14)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeTwoPartConditionalWithNestedNamedGroups()
        {
            // Arrange
            const string input = "(?(foo)(?<bar>baz)|(?<bar>baz))";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ConditionalExpressionStart, "(", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 6),
                    new Token(TokenType.GroupStart, "(", 7),
                    new Token(TokenType.GroupDirectiveStart, "?", 8),
                    new Token(TokenType.NamedIdentifierStart, "<", 9),
                    new Token(TokenType.Literal, "b", 10),
                    new Token(TokenType.Literal, "a", 11),
                    new Token(TokenType.Literal, "r", 12),
                    new Token(TokenType.NamedIdentifierEnd, ">", 13),
                    new Token(TokenType.Literal, "b", 14),
                    new Token(TokenType.Literal, "a", 15),
                    new Token(TokenType.Literal, "z", 16),
                    new Token(TokenType.GroupEnd, ")", 17),
                    new Token(TokenType.OrOperator, "|", 18),
                    new Token(TokenType.GroupStart, "(", 19),
                    new Token(TokenType.GroupDirectiveStart, "?", 20),
                    new Token(TokenType.NamedIdentifierStart, "<", 21),
                    new Token(TokenType.Literal, "b", 22),
                    new Token(TokenType.Literal, "a", 23),
                    new Token(TokenType.Literal, "r", 24),
                    new Token(TokenType.NamedIdentifierEnd, ">", 25),
                    new Token(TokenType.Literal, "b", 26),
                    new Token(TokenType.Literal, "a", 27),
                    new Token(TokenType.Literal, "z", 28),
                    new Token(TokenType.GroupEnd, ")", 29),
                    new Token(TokenType.GroupEnd, ")", 30)
                },
                result.ToArray()
            );
        }
    }
}
