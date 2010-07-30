using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void Tokenizer_GetTokens_ShouldReturnLiteralTokenForSingleCharacter()
        {
            // Arrange
            const string input = "a";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new LiteralToken("a", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldReturnMultipleLiteralTokensForMultipleCharacters()
        {
            // Arrange
            const string input = "abc";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new LiteralToken("a", 0),
                    new LiteralToken("b", 1),
                    new LiteralToken("c", 2)
                },
                result.ToArray()
            );
        }
    }
}