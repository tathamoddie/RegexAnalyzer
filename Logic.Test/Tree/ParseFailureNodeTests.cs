using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;
using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tree
{
    [TestClass]
    public class ParseFailureNodeTests
    {
        [TestMethod]
        public void ParseFailureNode_Description_PrependsErrorToMessage()
        {
            var token = new Token(TokenType.Number, "1", 0);
            var node = new ParseFailureNode(token, "message");
            Assert.AreEqual("error: message", node.Description);
        }

        [TestMethod]
        public void ParseFailureNode_Equals_ShouldReturnTrueWhenMessagesAreTheSame()
        {
            var lhs = new ParseFailureNode("(abc)", 0, "message");
            var rhs = new ParseFailureNode("(abc)", 0, "message");
            Assert.IsTrue(lhs.Equals(rhs));
        }

        [TestMethod]
        public void ParseFailureNode_Equals_ShouldReturnFalseWhenMessageDiffers()
        {
            var lhs = new ParseFailureNode("(abc)", 0, "message 1");
            var rhs = new ParseFailureNode("(abc)", 0, "message 2");
            Assert.IsFalse(lhs.Equals(rhs));
        }
    }
}