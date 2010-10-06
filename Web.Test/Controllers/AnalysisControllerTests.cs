using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tree;
using TathamOddie.RegexAnalyzer.Web.Controllers;

namespace TathamOddie.RegexAnalyzer.Web.Test.Controllers
{
    [TestClass]
    public class AnalysisControllerTests
    {
        class TestNode : Node
        {
            public TestNode(string data, int startIndex)
                : base(data, startIndex)
            {}

            public TestNode(string data, int startIndex, params Node[] children)
                : base(data, startIndex)
            {
                AddChildren(children);
            }

            public override string Description
            {
                get { return StartIndex.ToString(); }
            }
        }

        [TestMethod]
        public void AnalysisController_BuildNodeClass_ShouldReturnDefaultClassesWithNodeId()
        {
            // Arrange
            var node = new TestNode("abc", 0) { NodeId = 456 };

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
            var nodes = new TestNode("abc", 0, new Node[]
            {
                new TestNode("abc", 0) { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">abc</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderContentBeforeAllNodes()
        {
            // Arrange
            var nodes = new TestNode("abcdef", 0, new Node[]
            {
                new TestNode("def", 3) { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("abc<span class=\"ast-node ast-node-1\" title=\"3\">def</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderContentAfterAllNodes()
        {
            // Arrange
            var nodes = new TestNode("abcdef", 0, new Node[]
            {
                new TestNode("abc", 0) { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">abc</span>def", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderContentBeforeAndAfterAllNodes()
        {
            // Arrange
            var nodes = new TestNode("abcdefghi", 0, new Node[]
            {
                new TestNode("def", 3) { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("abc<span class=\"ast-node ast-node-1\" title=\"3\">def</span>ghi", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldEncodeDataBeforeBetweenAfterAndWithinNodes()
        {
            // Arrange
            var nodes = new TestNode("<<<<<", 0, new Node[]
            {
                new TestNode("<", 1) { NodeId = 1 },
                new TestNode("<", 3) { NodeId = 2 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("&lt;<span class=\"ast-node ast-node-1\" title=\"1\">&lt;</span>&lt;<span class=\"ast-node ast-node-2\" title=\"3\">&lt;</span>&lt;", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderSequentialFlatNodes()
        {
            // Arrange
            var nodes = new TestNode("abcdef", 0, new Node[]
            {
                new TestNode("abc", 0) { NodeId = 1 },
                new TestNode("def", 3) { NodeId = 2 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">abc</span><span class=\"ast-node ast-node-2\" title=\"3\">def</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderSiblingGroups()
        {
            // Arrange
            var nodes = new TestNode("(abc)(def)", 0, new Node[]
            {
                new TestNode("(abc)", 0,
                    new TestNode("abc", 1) { NodeId = 3 })
                { NodeId = 1 },
                
                new TestNode("(def)", 5,
                    new TestNode("def", 6) { NodeId = 4 })
                { NodeId = 2 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">(<span class=\"ast-node ast-node-3\" title=\"1\">abc</span>)</span><span class=\"ast-node ast-node-2\" title=\"5\">(<span class=\"ast-node ast-node-4\" title=\"6\">def</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderNestedEmptyGroups()
        {
            // Arrange
            var nodes = new TestNode("(())", 0, new Node[]
            {
                new TestNode("(())", 0,
                    new TestNode("()", 1) { NodeId = 2 })
                { NodeId = 1 },
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">(<span class=\"ast-node ast-node-2\" title=\"1\">()</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderNestedGroupsWhereNode3HasChildren()
        {
            // Arrange
            var nodes = new TestNode("(foo(bar))", 0, new Node[]
            {
                new TestNode("(foo(bar))", 0,
                    new TestNode("foo", 1) { NodeId = 2 },
                    new TestNode("(bar)", 4,
                        new TestNode("bar", 5) { NodeId = 4 })
                    { NodeId = 3 })
                { NodeId = 1 },
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">(<span class=\"ast-node ast-node-2\" title=\"1\">foo</span><span class=\"ast-node ast-node-3\" title=\"4\">(<span class=\"ast-node ast-node-4\" title=\"5\">bar</span>)</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderDoubleNestedGroupsWithTrailingContent()
        {
            // Arrange
            var nodes = new TestNode("((abc))def", 0, new Node[]
            {
                new TestNode("((abc))", 0,
                    new TestNode("(abc)", 1,
                        new TestNode("abc", 2) { NodeId = 4 })
                    { NodeId = 3 })
                { NodeId = 1 },
                new TestNode("def", 7) { NodeId = 2 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">(<span class=\"ast-node ast-node-3\" title=\"1\">(<span class=\"ast-node ast-node-4\" title=\"2\">abc</span>)</span>)</span><span class=\"ast-node ast-node-2\" title=\"7\">def</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodesWithDataBeforeChildNode()
        {
            // Arrange
            var nodes = new TestNode("(abc", 0, new Node[]
            {
                new TestNode("(abc", 0,
                    new TestNode("abc", 1) { NodeId = 2 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">(<span class=\"ast-node ast-node-2\" title=\"1\">abc</span></span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodesWithDataAfterChildNode()
        {
            // Arrange
            var nodes = new TestNode("abc)", 0, new Node[]
            {
                new TestNode("abc)", 0,
                    new TestNode("abc", 0) { NodeId = 2 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\"><span class=\"ast-node ast-node-2\" title=\"0\">abc</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodesWithDataBeforeAndAfterChildNode()
        {
            // Arrange
            var nodes = new TestNode("(abc)", 0, new Node[]
            {
                new TestNode("(abc)", 0,
                    new TestNode("abc", 1) { NodeId = 2 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">(<span class=\"ast-node ast-node-2\" title=\"1\">abc</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodesWithDataBetweenChildNodes()
        {
            // Arrange
            var nodes = new TestNode("abc|def", 0, new Node[]
            {
                new TestNode("abc|def", 0,
                    new TestNode("abc", 0) { NodeId = 2 },
                    new TestNode("def", 4) { NodeId = 3 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\"><span class=\"ast-node ast-node-2\" title=\"0\">abc</span>|<span class=\"ast-node ast-node-3\" title=\"4\">def</span></span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderGroupNodesWithDataBeforeAndAfterAndBetweenChildNodes()
        {
            // Arrange
            var nodes = new TestNode("(abc|def)", 0, new Node[]
            {
                new TestNode("(abc|def)", 0,
                    new TestNode("abc", 1) { NodeId = 2 },
                    new TestNode("def", 5) { NodeId = 3 })
                { NodeId = 1 }
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            Assert.AreEqual("<span class=\"ast-node ast-node-1\" title=\"0\">(<span class=\"ast-node ast-node-2\" title=\"1\">abc</span>|<span class=\"ast-node ast-node-3\" title=\"5\">def</span>)</span>", result);
        }

        [TestMethod]
        public void AnalysisController_RenderExpressionAsHtml_ShouldRenderNestedGroupNodesWithDataBeforeAndAfterAndBetweenChildNodes()
        {
            // Arrange
            var nodes = new TestNode("abc(def(ghi)jkl(mno)pqr)stu", 0, new Node[]
            {
                new TestNode("abc", 0),
                new TestNode("(def(ghi)jkl(mno)pqr)", 3,
                    new TestNode("def", 4),
                    new TestNode("(ghi)", 7,
                        new TestNode("ghi", 8)),
                    new TestNode("jkl", 12),
                    new TestNode("(mno)", 15,
                        new TestNode("mno", 16)),
                    new TestNode("pqr", 20)),
                new TestNode("stu", 24)
            });

            // Act
            var result = AnalysisController.RenderExpressionAsHtml(nodes).ToHtmlString();

            // Assert
            const string expected =
                "<span class=\"ast-node ast-node-0\" title=\"0\">abc</span>" +
                "<span class=\"ast-node ast-node-0\" title=\"3\">(" +
                    "<span class=\"ast-node ast-node-0\" title=\"4\">def</span>" +
                    "<span class=\"ast-node ast-node-0\" title=\"7\">(" +
                        "<span class=\"ast-node ast-node-0\" title=\"8\">ghi</span>" +
                    ")</span>" +
                    "<span class=\"ast-node ast-node-0\" title=\"12\">jkl</span>" +
                    "<span class=\"ast-node ast-node-0\" title=\"15\">(" +
                        "<span class=\"ast-node ast-node-0\" title=\"16\">mno</span>" +
                    ")</span>" +
                    "<span class=\"ast-node ast-node-0\" title=\"20\">pqr</span>" +
                ")</span>" +
                "<span class=\"ast-node ast-node-0\" title=\"24\">stu</span>";
            Assert.AreEqual(expected, result);
        }
    }
}