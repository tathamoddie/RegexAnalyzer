using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TathamOddie.RegexAnalyzer.Logic.Test
{
    [TestClass]
    public class QueueExtensionTests
    {
        [TestMethod]
        public void QueueExtensions_DequeuePattern_ShouldDequeueSingleElementPattern()
        {
            // Arrange
            var input = new Queue<string>(new[]
            {
                "a",
                "b",
                "c"
            });

            // Act
            var result = input.DequeuePattern(new[]
            {
                new PatternSegment<string>(s => s == "a", 1)
            });

            // Assert
            CollectionAssert.AreEqual(
                new [] { "a" },
                result.ToArray());
            Assert.AreEqual(2, input.Count());
        }

        [TestMethod]
        public void QueueExtensions_DequeuePattern_ShouldDequeueTwoElementPattern()
        {
            // Arrange
            var input = new Queue<string>(new[]
            {
                "a",
                "b",
                "c"
            });

            // Act
            var result = input.DequeuePattern(new[]
            {
                new PatternSegment<string>(s => s == "a", 1),
                new PatternSegment<string>(s => s == "b", 1)
            });

            // Assert
            CollectionAssert.AreEqual(
                new[] { "a", "b" },
                result.ToArray());
            Assert.AreEqual(1, input.Count());
        }

        [TestMethod]
        public void QueueExtensions_DequeuePattern_ShouldDequeueTwoElementPatternWithCounts()
        {
            // Arrange
            var input = new Queue<string>(new[]
            {
                "a",
                "a",
                "a",
                "b",
                "b",
                "c"
            });

            // Act
            var result = input.DequeuePattern(new[]
            {
                new PatternSegment<string>(s => s == "a", 3),
                new PatternSegment<string>(s => s == "b", 2)
            });

            // Assert
            CollectionAssert.AreEqual(
                new[] { "a", "a", "a", "b", "b" },
                result.ToArray());
            Assert.AreEqual(1, input.Count());
        }

        [TestMethod]
        public void QueueExtensions_DequeuePattern_ShouldDequeueTwoElementPatternWithMultiRead()
        {
            // Arrange
            var input = new Queue<string>(new[]
            {
                "a",
                "a",
                "b",
                "b",
                "c"
            });

            // Act
            var result = input.DequeuePattern(new[]
            {
                new PatternSegment<string>(s => s == "a", 2),
                new PatternSegment<string>(s => s == "b")
            });

            // Assert
            CollectionAssert.AreEqual(
                new[] { "a", "a", "b", "b" },
                result.ToArray());
            Assert.AreEqual(1, input.Count());
        }

        [TestMethod]
        public void QueueExtensions_DequeuePattern_ShouldReturnEmptyEnumerableWhenPatternFails()
        {
            // Arrange
            var input = new Queue<string>(new[]
            {
                "a",
                "a",
                "b",
                "b",
                "c"
            });

            // Act
            var result = input.DequeuePattern(new[]
            {
                new PatternSegment<string>(s => s == "a", 2),
                new PatternSegment<string>(s => s == "c")
            });

            // Assert
            Assert.AreEqual(0, result.Count());
            Assert.AreEqual(5, input.Count());
        }
    }
}