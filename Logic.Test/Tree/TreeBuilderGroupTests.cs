using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class TreeBuilderGroupTests
    {
        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildEmptyGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupEnd, ")", 1),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("()", 0)
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildParseFailureNodeForUnclosedGroup()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new ParseFailureNode("(", 0, "group is never closed")
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildGroupNodeWithLiteralContent()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.Literal, "abc", 1),
                new Token(TokenType.GroupEnd, ")", 4),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(abc)", 0,
                        new LiteralNode("abc", 1))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildNestedGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupStart, "(", 1),
                new Token(TokenType.GroupEnd, ")", 2),
                new Token(TokenType.GroupEnd, ")", 3),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(())", 0,
                        new GroupNode("()", 1))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildNestedGroupNodeWithLiteralContent()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.Literal, "abc", 1),
                new Token(TokenType.GroupStart, "(", 4),
                new Token(TokenType.Literal, "def", 5),
                new Token(TokenType.GroupEnd, ")", 8),
                new Token(TokenType.Literal, "ghi", 9),
                new Token(TokenType.GroupEnd, ")", 12),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(abc(def)ghi)", 0,
                        new LiteralNode("abc", 1),
                        new GroupNode("(def)", 4,
                            new LiteralNode("def", 5)),
                        new LiteralNode("ghi", 9))
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildNamedGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupDirectiveStart, "?", 1),
                new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 2),
                new Token(TokenType.Literal, "foo", 3),
                new Token(TokenType.NamedIdentifierEnd, ">", 6),
                new Token(TokenType.Literal, "bar", 7),
                new Token(TokenType.GroupEnd, ")", 10)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(?<foo>bar)", 0,
                        new LiteralNode("bar", 7))
                    { NamedIdentifier = "foo" }
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildBalancingGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupDirectiveStart, "?", 1),
                new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 2),
                new Token(TokenType.Literal, "foo", 3),
                new Token(TokenType.BalancingGroupNamedIdentifierSeparator, "-", 6),
                new Token(TokenType.Literal, "bar", 7),
                new Token(TokenType.NamedIdentifierEnd, ">", 10),
                new Token(TokenType.Literal, "baz", 11),
                new Token(TokenType.GroupEnd, ")", 14)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(?<foo-bar>baz)", 0,
                        new LiteralNode("baz", 11))
                    { NamedIdentifier = "foo", BalancingGroupIdentifier = "bar" }
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildCapturingGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.Literal, "abc", 1),
                new Token(TokenType.GroupEnd, ")", 4),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(abc)", 0,
                        new LiteralNode("abc", 1))
                    { GroupMode = GroupMode.CapturingGroup }
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildNonCapturingGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupDirectiveStart, "?", 1),
                new Token(TokenType.NonCapturingGroupMarker, ":", 2),
                new Token(TokenType.Literal, "abc", 3),
                new Token(TokenType.GroupEnd, ")", 6),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(?:abc)", 0,
                        new LiteralNode("abc", 3))
                    { GroupMode = GroupMode.NonCapturingGroup }
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildPositiveLookAheadGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupDirectiveStart, "?", 1),
                new Token(TokenType.PositiveLookAheadMarker, "=", 2),
                new Token(TokenType.Literal, "abc", 3),
                new Token(TokenType.GroupEnd, ")", 6),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(?=abc)", 0,
                        new LiteralNode("abc", 3))
                    { GroupMode = GroupMode.PositiveLookAhead }
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildPositiveLookBehindGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupDirectiveStart, "?", 1),
                new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 2),
                new Token(TokenType.PositiveLookBehindMarker, "=", 3),
                new Token(TokenType.Literal, "abc", 4),
                new Token(TokenType.GroupEnd, ")", 7),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(?<=abc)", 0,
                        new LiteralNode("abc", 4))
                    { GroupMode = GroupMode.PositiveLookBehind }
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildNegativeLookAheadGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupDirectiveStart, "?", 1),
                new Token(TokenType.NegativeLookAheadMarker, "!", 2),
                new Token(TokenType.Literal, "abc", 3),
                new Token(TokenType.GroupEnd, ")", 6),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(?!abc)", 0,
                        new LiteralNode("abc", 3))
                    { GroupMode = GroupMode.NegativeLookAhead }
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldBuildNegativeLookBehindGroupNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.GroupStart, "(", 0),
                new Token(TokenType.GroupDirectiveStart, "?", 1),
                new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 2),
                new Token(TokenType.NegativeLookBehindMarker, "!", 3),
                new Token(TokenType.Literal, "abc", 4),
                new Token(TokenType.GroupEnd, ")", 7),
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new GroupNode("(?<!abc)", 0,
                        new LiteralNode("abc", 4))
                    { GroupMode = GroupMode.NegativeLookBehind }
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilding_Build_ShouldNotThrowStackOverflowExceptionForMassivelyNestedGroups()
        {
            // Arrange
            const int depth = 1000;
            var startTokens = Enumerable.Range(0, depth)
                .Select(i => new Token(TokenType.GroupStart, "(", i));
            var endTokens = Enumerable.Range(depth, depth)
                .Select(i => new Token(TokenType.GroupEnd, ")", i));
            var tokens = startTokens.Concat(endTokens);

            // Act
            new TreeBuilder().Build(tokens);

            // Assert
        }
    }
}