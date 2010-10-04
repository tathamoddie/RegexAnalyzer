using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class NodeTests
    {
        public class TestNode : Node
        {
            public TestNode(string data, int startIndex)
                : base(data, startIndex)
            {
            }

            public override string Description
            {
                get { throw new NotImplementedException(); }
            }
        }

        [TestMethod]
        public void Node_Equals_ShouldReturnTrueForDefaultConstructor()
        {
            var lhs = new TestNode("abc", 0);
            var rhs = new TestNode("abc", 0);
            Assert.AreEqual(lhs, rhs);
        }

        [TestMethod]
        public void Node_Equals_ShouldReturnFalseWhenNodeIdsDiffer()
        {
            var lhs = new TestNode("abc", 0) { NodeId = 123 };
            var rhs = new TestNode("abc", 0) { NodeId = 456 };
            Assert.AreNotEqual(lhs, rhs);
        }
    }
}