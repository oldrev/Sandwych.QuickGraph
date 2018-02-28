using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph.Algorithms;
using QuickGraph.Serialization;
using Xunit;

namespace QuickGraph.Tests.Algorithms
{
    public class AlgorithmExtensionsTest
    {
        [Fact]
        public void AdjacencyGraphRoots()
        {
            var g = new AdjacencyGraph<string, Edge<string>>();
            g.AddVertex("A");
            g.AddVertex("B");
            g.AddVertex("C");

            g.AddEdge(new Edge<string>("A", "B"));
            g.AddEdge(new Edge<string>("B", "C"));

            var roots = g.Roots().ToList();
            Assert.Single(roots);
            Assert.Equal("A", roots[0]);
        }

        [Fact]
        public void AllAdjacencyGraphRoots()
        {
            foreach (var graphmlFile in TestGraphFactory.GetFileNames())
            {
                var g = TestGraphFactory.LoadGraph(graphmlFile);
                Roots(g);
            }
        }

        private void Roots<T>(IVertexAndEdgeListGraph<T, Edge<T>> g)
        {
            var roots = new HashSet<T>(g.Roots());
            foreach (var edge in g.Edges)
                Assert.Contains(edge.Target, roots);
        }
    }
}
