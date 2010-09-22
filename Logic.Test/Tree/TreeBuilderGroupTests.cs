using System;
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
                    new GroupNode(@"()", 0)
                },
                nodes.ToArray()
            );
        }
    }
}