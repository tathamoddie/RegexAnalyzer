using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class GroupNodeTests
    {
        [TestMethod]
        public void GroupNode_Description_ShouldReturnGroupForSimpleCaptureGroup()
        {
            var node = new GroupNode("(abc)", 0);
            Assert.AreEqual("group", node.Description);
        }

        [TestMethod]
        public void GroupNode_Description_ShouldReturnNamedGroupWithIdentifier()
        {
            var node = new GroupNode("(abc)", 0) { NamedIdentifier = "foo" };
            Assert.AreEqual("named group 'foo'", node.Description);
        }

        [TestMethod]
        public void GroupNode_Description_ShouldReturnBalancingGroup()
        {
            var node = new GroupNode("(abc)", 0)
                           {
                               BalancingGroupIdentifier = "foo",
                               NamedIdentifier = "bar"
                           };
            Assert.AreEqual("balancing group 'foo', captured in 'bar'", node.Description);
        }

        [TestMethod]
        public void GroupNode_Equals_ShouldReturnTrueWithDefaultConstructor()
        {
            var lhs = new GroupNode("(abc)", 0);
            var rhs = new GroupNode("(abc)", 0);
            Assert.IsTrue(lhs.Equals(rhs));
        }

        [TestMethod]
        public void GroupNode_Equals_ShouldReturnFalseWhenIsCapturingDiffers()
        {
            var lhs = new GroupNode("(abc)", 0) { IsCapturing = true };
            var rhs = new GroupNode("(abc)", 0) { IsCapturing = false };
            Assert.IsFalse(lhs.Equals(rhs));
        }

        [TestMethod]
        public void GroupNode_Equals_ShouldReturnFalseWhenNamedIdentifierDiffers()
        {
            var lhs = new GroupNode("(abc)", 0) { NamedIdentifier = "abc" };
            var rhs = new GroupNode("(abc)", 0) { NamedIdentifier = "def" };
            Assert.IsFalse(lhs.Equals(rhs));
        }

        [TestMethod]
        public void GroupNode_Equals_ShouldReturnFalseWhenBalancingGroupIdentifierDiffers()
        {
            var lhs = new GroupNode("(abc)", 0) { BalancingGroupIdentifier = "abc" };
            var rhs = new GroupNode("(abc)", 0) { BalancingGroupIdentifier = "def" };
            Assert.IsFalse(lhs.Equals(rhs));
        }
    }
}