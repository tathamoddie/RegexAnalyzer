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
        public void TreeBuilder_Build_ShouldBuildZeroOrMoreQualifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.Quantifier, "*", 1)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

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
        public void TreeBuilder_Build_ShouldBuildOneOrMoreQualifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.Quantifier, "+", 1)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

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
        public void TreeBuilder_Build_ShouldBuildZeroOrOneQualifierNodeForSingleCharacterLiteral()
        {
            // Arrange
            var tokens = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.Quantifier, "?", 1)
            };

            // Act
            var nodes = new TreeBuilder().Build(tokens);

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
    }
}