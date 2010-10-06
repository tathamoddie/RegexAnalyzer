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

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderSingleFlatNode()
        {
            // Arrange
            var nodes = new ExpressionNode("abc", 0, new Node[]
            {
                new LiteralNode("abc", 0) { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\">abc</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderContentBeforeAllNodes()
        {
            // Arrange
            var nodes = new ExpressionNode("abcdef", 0, new Node[]
            {
                new LiteralNode("def", 3) { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("abc<span class=\"ast-node ast-node-1\">def</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderContentAfterAllNodes()
        {
            // Arrange
            var nodes = new ExpressionNode("abcdef", 0, new Node[]
            {
                new LiteralNode("abc", 0) { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\">abc</span>def", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderContentBeforeAndAfterAllNodes()
        {
            // Arrange
            var nodes = new ExpressionNode("abcdefghi", 0, new Node[]
            {
                new LiteralNode("def", 3) { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("abc<span class=\"ast-node ast-node-1\">def</span>ghi", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldEncodeDataBeforeBetweenAfterAndWithinNodes()
        {
            // Arrange
            var nodes = new ExpressionNode("<<<<<", 0, new Node[]
            {
                new LiteralNode("<", 1) { NodeId = 1 },
                new LiteralNode("<", 3) { NodeId = 2 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("&lt;<span class=\"ast-node ast-node-1\">&lt;</span>&lt;<span class=\"ast-node ast-node-2\">&lt;</span>&lt;", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderSequentialFlatNodes()
        {
            // Arrange
            var nodes = new ExpressionNode("abcdef", 0, new Node[]
            {
                new LiteralNode("abc", 0) { NodeId = 1 },
                new LiteralNode("def", 3) { NodeId = 2 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\">abc</span><span class=\"ast-node ast-node-2\">def</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderSiblingGroups()
        {
            // Arrange
            var nodes = new ExpressionNode("(abc)(def)", 0, new Node[]
            {
                new GroupNode("(abc)", 0,
                    new LiteralNode("abc", 1) { NodeId = 3 })
                { NodeId = 1 },
                
                new GroupNode("(def)", 5,
                    new LiteralNode("def", 6) { NodeId = 4 })
                { NodeId = 2 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\">(<span class=\"ast-node ast-node-3\">abc</span>)</span><span class=\"ast-node ast-node-2\">(<span class=\"ast-node ast-node-4\">def</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderSimpleNestedGroup()
        {
            // Arrange
            var nodes = new ExpressionNode("(foo(bar))", 0, new Node[]
            {
                new GroupNode("(foo(bar))", 0,
                    new LiteralNode("foo", 1) { NodeId = 2 },
                    new GroupNode("(bar)", 4,
                        new LiteralNode("bar", 5) { NodeId = 4 })
                    { NodeId = 3 })
                { NodeId = 1 },
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\">(<span class=\"ast-node ast-node-2\">foo</span><span class=\"ast-node ast-node-3\">(<span class=\"ast-node ast-node-4\">bar</span>)</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodeWithDataBeforeChildNode()
        {
            // Arrange
            var nodes = new ExpressionNode("(abc", 0, new Node[]
            {
                new GroupNode("(abc", 0,
                    new LiteralNode("abc", 1) { NodeId = 2 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\">(<span class=\"ast-node ast-node-2\">abc</span></span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodeWithDataAfterChildNode()
        {
            // Arrange
            var nodes = new ExpressionNode("abc)", 0, new Node[]
            {
                new GroupNode("abc)", 0,
                    new LiteralNode("abc", 0) { NodeId = 2 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\"><span class=\"ast-node ast-node-2\">abc</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodeWithDataBeforeAndAfterChildNode()
        {
            // Arrange
            var nodes = new ExpressionNode("(abc)", 0, new Node[]
            {
                new GroupNode("(abc)", 0,
                    new LiteralNode("abc", 1) { NodeId = 2 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\">(<span class=\"ast-node ast-node-2\">abc</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodeWithDataBetweenChildNodes()
        {
            // Arrange
            var nodes = new ExpressionNode("abc|def", 0, new Node[]
            {
                new GroupNode("abc|def", 0,
                    new LiteralNode("abc", 0) { NodeId = 2 },
                    new LiteralNode("def", 4) { NodeId = 3 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\"><span class=\"ast-node ast-node-2\">abc</span>|<span class=\"ast-node ast-node-3\">def</span></span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodeWithDataBeforeAndAfterAndBetweenChildNodes()
        {
            // Arrange
            var nodes = new ExpressionNode("(abc|def)", 0, new Node[]
            {
                new GroupNode("(abc|def)", 0,
                    new LiteralNode("abc", 1) { NodeId = 2 },
                    new LiteralNode("def", 5) { NodeId = 3 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\">(<span class=\"ast-node ast-node-2\">abc</span>|<span class=\"ast-node ast-node-3\">def</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderNestedGroupNodesWithDataBeforeAndAfterAndBetweenChildNodes()
        {
            // Arrange
            var nodes = new ExpressionNode("abc(def(ghi)jkl(mno)pqr)stu", 0, new Node[]
            {
                new LiteralNode("abc", 0),
                new GroupNode("(def(ghi)jkl(mno)pqr)", 3,
                    new LiteralNode("def", 4),
                    new GroupNode("(ghi)", 7,
                        new LiteralNode("ghi", 8)),
                    new LiteralNode("jkl", 12),
                    new GroupNode("(mno)", 15,
                        new LiteralNode("mno", 16)),
                    new LiteralNode("pqr", 20)),
                new LiteralNode("stu", 24)
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            const string expected =
                "<span class=\"ast-node ast-node-0\">abc</span>" +
                "<span class=\"ast-node ast-node-0\">(" +
                    "<span class=\"ast-node ast-node-0\">def</span>" +
                    "<span class=\"ast-node ast-node-0\">(" +
                        "<span class=\"ast-node ast-node-0\">ghi</span>" +
                    ")</span>" +
                    "<span class=\"ast-node ast-node-0\">jkl</span>" +
                    "<span class=\"ast-node ast-node-0\">(" +
                        "<span class=\"ast-node ast-node-0\">mno</span>" +
                    ")</span>" +
                    "<span class=\"ast-node ast-node-0\">pqr</span>" +
                ")</span>" +
                "<span class=\"ast-node ast-node-0\">stu</span>";
            Assert.AreEqual(expected, result);
        }
    }
}