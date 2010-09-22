﻿using System.Linq;
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