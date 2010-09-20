using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tokens
{
    [TestClass]
    public class TokenizerOptionTests
    {
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
    }
}