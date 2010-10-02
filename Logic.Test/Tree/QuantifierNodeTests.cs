using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class QuantifierNodeTests
    {
        [TestMethod]
        public void QuantifierNode_Description_0_0()
        {
            QuantifierNode_Description_Test(0, 0, "never occurs");
        }

        [TestMethod]
        public void QuantifierNode_Description_0_1()
        {
            QuantifierNode_Description_Test(0, 1, "zero or one occurences");
        }

        [TestMethod]
        public void QuantifierNode_Description_0_2()
        {
            QuantifierNode_Description_Test(0, 2, "up to two occurences");
        }

        [TestMethod]
        public void QuantifierNode_Description_0_3()
        {
            QuantifierNode_Description_Test(0, 3, "up to 3 occurences");
        }
        
        [TestMethod]
        public void QuantifierNode_Description_0_1234()
        {
            QuantifierNode_Description_Test(0, 1234, "up to 1,234 occurences");
        }

        [TestMethod]
        public void QuantifierNode_Description_1234_1234()
        {
            QuantifierNode_Description_Test(1234, 1234, "exactly 1,234 occurences");
        }

        [TestMethod]
        public void QuantifierNode_Description_1_1()
        {
            QuantifierNode_Description_Test(1, 1, "exactly once");
        }

        [TestMethod]
        public void QuantifierNode_Description_1_2()
        {
            QuantifierNode_Description_Test(1, 2, "one or two occurences");
        }

        [TestMethod]
        public void QuantifierNode_Description_1_3()
        {
            QuantifierNode_Description_Test(1, 3, "between 1 and 3 occurences");
        }

        [TestMethod]
        public void QuantifierNode_Description_1234_5678()
        {
            QuantifierNode_Description_Test(1234, 5678, "between 1,234 and 5,678 occurences");
        }

        static void QuantifierNode_Description_Test(int? min, int? max, string expected)
        {
            var node = new QuantifierNode("data", 0, min, max, new ExpressionNode("data", 0));
            Assert.AreEqual(expected, node.Description);
        }
    }
}