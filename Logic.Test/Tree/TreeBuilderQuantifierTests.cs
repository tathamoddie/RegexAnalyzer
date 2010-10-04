using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class TreeBuilderQuantifierTests
    {
        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildZeroOrMoreQuantifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.Quantifier, "*", 1)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new QuantifierNode("a*", 0,
                        0, null,
                        new LiteralNode("a", 0))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildOneOrMoreQuantifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.Quantifier, "+", 1)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new QuantifierNode("a+", 0,
                        1, null,
                        new LiteralNode("a", 0))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildZeroOrOneQuantifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.Quantifier, "?", 1)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new QuantifierNode("a?", 0,
                        0, 1,
                        new LiteralNode("a", 0))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildMinParametizedQuantifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.ParametizedQuantifierStart, "{", 1),
                new Token(TokenType.Number, "3", 2),
                new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 3),
                new Token(TokenType.ParametizedQuantifierEnd, "}", 4)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new QuantifierNode("a{3,}", 0,
                        3, null,
                        new LiteralNode("a", 0))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildMaxParametizedQuantifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.ParametizedQuantifierStart, "{", 1),
                new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 2),
                new Token(TokenType.Number, "3", 3),
                new Token(TokenType.ParametizedQuantifierEnd, "}", 4)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new QuantifierNode("a{,3}", 0,
                        null, 3,
                        new LiteralNode("a", 0))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildMinMaxParametizedQuantifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.ParametizedQuantifierStart, "{", 1),
                new Token(TokenType.Number, "3", 2),
                new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 3),
                new Token(TokenType.Number, "16", 4),
                new Token(TokenType.ParametizedQuantifierEnd, "}", 6)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new QuantifierNode("a{3,16}", 0,
                        3, 16,
                        new LiteralNode("a", 0))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildExactParametizedQuantifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.ParametizedQuantifierStart, "{", 1),
                new Token(TokenType.Number, "8", 2),
                new Token(TokenType.ParametizedQuantifierEnd, "}", 3)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new QuantifierNode("a{8}", 0,
                        8, 8,
                        new LiteralNode("a", 0))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldHandleMalformedParametizedQuantifierTokensAsLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.ParametizedQuantifierStart, "{", 1),
                new Token(TokenType.Number, "8", 2),
                new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 3),
                new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 4),
                new Token(TokenType.ParametizedQuantifierEnd, "}", 5)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new LiteralNode("a", 0),
                    new LiteralNode("{8,,}", 1)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildQuantifierNodeAroundLastCharacterOfMultiCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "abc", 0),
                new Token(TokenType.Quantifier, "*", 1)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new Node[]
                {
                    new LiteralNode("ab", 0),
                    new QuantifierNode("c*", 2,
                        0, null,
                        new LiteralNode("c", 2))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildMinMaxParametizedQuantifierNodeAroundLastCharacterOfMultiCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "abc", 0),
                new Token(TokenType.ParametizedQuantifierStart, "{", 3),
                new Token(TokenType.Number, "3", 4),
                new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 5),
                new Token(TokenType.Number, "16", 6),
                new Token(TokenType.ParametizedQuantifierEnd, "}", 8)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new Node[]
                {
                    new LiteralNode("ab", 0),
                    new QuantifierNode("c{3,16}", 2,
                        3, 16,
                        new LiteralNode("c", 2))
                },
                nodes.ToArray()
            );
        }
    }
}