using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tokens
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void TokenTests_Equals_Object_ShouldReturnFalseForNull()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = (Token) null;

            // Act
            var result = ((object)lhs).Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Object_ShouldReturnTrueForSameReference()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = lhs;

            // Act
            var result = ((object)lhs).Equals(rhs);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Object_ShouldReturnFalseForDifferentType()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Guid();

            // Act
            var result = lhs.Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Object_ShouldReturnTrueForSameValues()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "1", 0);

            // Act
            var result = ((object)lhs).Equals(rhs);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Object_ShouldReturnFalseForDifferentTokenType()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Literal, "1", 0);

            // Act
            var result = ((object)lhs).Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Object_ShouldReturnFalseForDifferentData()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "2", 0);

            // Act
            var result = ((object)lhs).Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Object_ShouldReturnFalseForDifferentIndex()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "1", 1);

            // Act
            var result = ((object)lhs).Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Equatable_ShouldReturnFalseForNull()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = (Token)null;

            // Act
            var result = ((IEquatable<Token>)lhs).Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Equatable_ShouldReturnTrueForSameReference()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = lhs;

            // Act
            var result = ((IEquatable<Token>)lhs).Equals(rhs);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Equatable_ShouldReturnTrueForSameValues()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "1", 0);

            // Act
            var result = ((IEquatable<Token>)lhs).Equals(rhs);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Equatable_ShouldReturnFalseForDifferentTokenType()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Literal, "1", 0);

            // Act
            var result = ((IEquatable<Token>)lhs).Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Equatable_ShouldReturnFalseForDifferentData()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "2", 0);

            // Act
            var result = ((IEquatable<Token>)lhs).Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_Equatable_ShouldReturnFalseForDifferentIndex()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "1", 1);

            // Act
            var result = ((IEquatable<Token>)lhs).Equals(rhs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenTests_Equals_GetHashCode_ShouldReturnSameCodeForSameValues()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "1", 0);

            // Act
            var result1 = lhs.GetHashCode();
            var result2 = rhs.GetHashCode();

            // Assert
            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        public void TokenTests_Equals_GetHashCode_ShouldReturnDifferentCodeForDifferentTokenType()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Literal, "1", 0);

            // Act
            var result1 = lhs.GetHashCode();
            var result2 = rhs.GetHashCode();

            // Assert
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void TokenTests_Equals_GetHashCode_ShouldReturnDifferentCodeForDifferentData()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "2", 0);

            // Act
            var result1 = lhs.GetHashCode();
            var result2 = rhs.GetHashCode();

            // Assert
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void TokenTests_Equals_GetHashCode_ShouldReturnDifferentCodeForDifferentIndex()
        {
            // Arrange
            var lhs = new Token(TokenType.Number, "1", 0);
            var rhs = new Token(TokenType.Number, "1", 1);

            // Act
            var result1 = lhs.GetHashCode();
            var result2 = rhs.GetHashCode();

            // Assert
            Assert.AreNotEqual(result1, result2);
        }
    }
}