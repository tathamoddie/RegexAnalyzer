using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class TreeBuilderCharacterTests
    {
        [TestMethod]
        public void TreeBuilder_Build_ShouldBuildWordCharacterClassNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.CharacterEscapeMarker, @"\", 10),
                new Token(TokenType.CharacterEscapeData, "w", 11)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new CharacterClassNode(@"\w", 10, CharacterClass.Word)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_Build_ShouldBuildNonWordCharacterClassNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.CharacterEscapeMarker, @"\", 10),
                new Token(TokenType.CharacterEscapeData, "W", 11)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new CharacterClassNode(@"\W", 10, CharacterClass.NonWord)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_Build_ShouldBuildWhiteSpaceCharacterClassNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.CharacterEscapeMarker, @"\", 10),
                new Token(TokenType.CharacterEscapeData, "s", 11)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new CharacterClassNode(@"\s", 10, CharacterClass.WhiteSpace)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_Build_ShouldBuildNonWhiteSpaceCharacterClassNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.CharacterEscapeMarker, @"\", 10),
                new Token(TokenType.CharacterEscapeData, "S", 11)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new CharacterClassNode(@"\S", 10, CharacterClass.NonWhiteSpace)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_Build_ShouldBuildDigitsCharacterClassNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.CharacterEscapeMarker, @"\", 10),
                new Token(TokenType.CharacterEscapeData, "d", 11)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new CharacterClassNode(@"\d", 10, CharacterClass.Digits)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_Build_ShouldBuildNonDigitsCharacterClassNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.CharacterEscapeMarker, @"\", 10),
                new Token(TokenType.CharacterEscapeData, "D", 11)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new CharacterClassNode(@"\D", 10, CharacterClass.NonDigits)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_Build_ShouldBuildCharacterEscapeIntoEscapeCharacterNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.CharacterEscapeMarker, @"\", 0),
                new Token(TokenType.CharacterEscapeData, ")", 1)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new EscapedCharacterNode(@"\)", 0, ")")
                },
                nodes.ToArray()
            );
        }
    }
}