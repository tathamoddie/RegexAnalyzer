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
    }
}