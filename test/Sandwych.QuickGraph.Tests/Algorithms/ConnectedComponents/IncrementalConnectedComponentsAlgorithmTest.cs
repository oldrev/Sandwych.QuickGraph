using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph.Algorithms;
using Xunit;

namespace QuickGraph.Tests.Algorithms.ConnectedComponents
{
    public class IncrementalConnectedComponentsAlgorithmTest
    {
        [Fact]
        public void IncrementalConnectedComponent()
        {
            var g = new AdjacencyGraph<int, SEquatableEdge<int>>();
            g.AddVertexRange(new int[] { 0, 1, 2, 3 });
            var components = AlgorithmExtensions.IncrementalConnectedComponents(g);

            var current = components();
            Assert.Equal(4, current.Key);

            g.AddEdge(new SEquatableEdge<int>(0, 1));
            current = components();
            Assert.Equal(3, current.Key);

            g.AddEdge(new SEquatableEdge<int>(2, 3));
            current = components();
            Assert.Equal(2, current.Key);

            g.AddEdge(new SEquatableEdge<int>(1, 3));
            current = components();
            Assert.Equal(1, current.Key);
        }
    }
}
