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
    }
}