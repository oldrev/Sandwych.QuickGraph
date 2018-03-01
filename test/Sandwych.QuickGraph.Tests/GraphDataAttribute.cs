using QuickGraph.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using Xunit.Sdk;

namespace QuickGraph
{
    public enum GraphType
    {
        AdjacencyGraph,
        BidirectionalGraph,
        UndirectedGraph
    }

    public sealed class GraphDataAttribute : DataAttribute
    {
        public GraphType Type { get; set; } = GraphType.AdjacencyGraph;
        public string FileName { get; set; } = null;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                foreach (var graphmlFile in TestGraphFactory.GetFileNames())
                {
                    var g = this.LoadSingleGraph(graphmlFile);
                    yield return new object[] { g };
                }
            }
            else
            {
                var path = Path.Combine("graphml", this.FileName);
                yield return new object[] { this.LoadSingleGraph(path) };
            }
        }

        private object LoadSingleGraph(string path)
        {
            switch (this.Type)
            {
                case GraphType.AdjacencyGraph: return TestGraphFactory.LoadGraph(path);
                case GraphType.BidirectionalGraph: return TestGraphFactory.LoadBidirectionalGraph(path);
                case GraphType.UndirectedGraph: return TestGraphFactory.LoadUndirectedGraph(path);
                default: throw new NotSupportedException();
            }
        }
    }
}
