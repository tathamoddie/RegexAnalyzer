﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class TreeBuilderTests
    {
        public void TreeBuilder_BuildTree_ShouldConvertParseFailureToken()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.ParseFailure, "x", 0)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new ParseFailureNode("x", 0, "Unrecognised token.")
                },
                nodes.ToArray()
            );
        }

        [TestMethod]
        public void TreeBuilder_Build_ShouldBuildLiteralTokenIntoLiteralNode()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "abc", 0)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new LiteralNode("abc", 0)
                },
                nodes.ToArray()
            );
        }
    }
}