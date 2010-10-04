using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tree;
using Web.Controllers;

namespace TathamOddie.RegexAnalyzer.Web.Test.Controllers
{
    [TestClass]
    public class AnalysisControllerTests
    {
        [TestMethod]
        public void AnalysisController_BuildNodeClass_ShouldReturnDefaultClassesWithNodeId()
        {
            // Arrange
            var node = new LiteralNode("abc", 0) { NodeId = 456 };

            // Act
            var nodeClass = AnalysisController.BuildNodeClass(node);

            // Assert
            Assert.AreEqual("ast-node ast-node-456", nodeClass);
        }

        [TestMethod]
        public void AnalysisController_BuildNodeClass_ShouldAddParseFailureClassForParseFailureNode()
        {
            // Arrange
            var node = new ParseFailureNode("abc", 0, "message");

            // Act
            var nodeClass = AnalysisController.BuildNodeClass(node);

            // Assert
            StringAssert.Contains(nodeClass, "ast-parse-failure-node");
        }
    }
}