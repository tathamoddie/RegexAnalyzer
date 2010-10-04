using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class TreeBuilderTests
    {
        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldConvertParseFailureToken()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.ParseFailure, "x", 0)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new ParseFailureNode("x", 0, "unrecognised token")
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldBuildLiteralTokenIntoLiteralNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "abc", 0)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new LiteralNode("abc", 0)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_BuildNodes_ShouldConvertUnexpectedTokenToParseFailureNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupEnd, ")", 0)
            };

            // Act
            var nodes = new TreeBuilder().BuildNodes(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new ParseFailureNode(")", 0, "unexpected token")
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_Build_ShouldAssignIds()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "1", 0),    // 1
                new Token(TokenType.Literal, "2", 1),    // 2
                new Token(TokenType.Literal, "3", 2),    // 3
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new Node[]
                {
                    new LiteralNode("1", 0) { NodeId = 1 },
                    new LiteralNode("2", 1) { NodeId = 2 },
                    new LiteralNode("3", 2) { NodeId = 3 },
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_AssignIdsToNodes_ShouldAssignIdsByLevel()
        {
            // Arrange
            var nodes = new Node[]
            {
                new LiteralNode("1", 0),            // 1
                new GroupNode("(23(45)67)", 1,      // 2
                    new LiteralNode("23", 2),       // 4
                    new GroupNode("(45)", 4,        // 5
                        new LiteralNode("45", 5)),  // 7
                    new LiteralNode("67", 8)),      // 6
                new LiteralNode("8", 11),           // 3
            };

            // Act
            TreeBuilder.AssignIdsToNodes(nodes);

            // Assert
            CollectionAssert.AreEqual(new Node[]
                {
                    new LiteralNode("1", 0) { NodeId = 1 },
                    new GroupNode("(23(45)67)", 1,
                        new LiteralNode("23", 2) { NodeId = 4 },
                        new GroupNode("(45)", 4,
                            new LiteralNode("45", 5) { NodeId = 7 })
                            { NodeId = 5 },
                        new LiteralNode("67", 8) { NodeId = 6 })
                        { NodeId = 2 },
                    new LiteralNode("8", 11) { NodeId = 3 },
                },
                nodes.ToArray()
            );
        }
    }
}