using TathamOddie.RegexAnalyzer.Logic.Tree;

namespace TathamOddie.RegexAnalyzer.Web.Models
{
    public class NodeViewModel
    {
        readonly Node node;

        public NodeViewModel(Node node)
        {
            this.node = node;
        }

        public int NodeId
        {
            get { return node.NodeId; }
        }

        public string TypeName
        {
            get { return node.GetType().Name; }
        }

        public string StartIndex
        {
            get { return node.StartIndex.ToString("#,##0"); }
        }

        public string Data
        {
            get { return node.Data; }
        }

        public int Depth { get; set; }
        public string CssClass { get; set; }
    }
}