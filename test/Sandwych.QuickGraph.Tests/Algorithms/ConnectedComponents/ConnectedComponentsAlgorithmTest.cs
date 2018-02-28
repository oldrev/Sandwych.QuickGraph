using System;
using System.Collections.Generic;
using QuickGraph.Serialization;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using QuickGraph.Algorithms.ConnectedComponents;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Algorithms.ConnectedComponents
{
    public class ConnectedComponentsAlgorithmTest
    {
        [Fact]
        public void ConnectedComponentsAll()
        {
            Parallel.ForEach(TestGraphFactory.GetUndirectedGraphs(), g =>
            {
                while (g.EdgeCount > 0)
                {
                    this.Compute(g);
                    g.RemoveEdge(Enumerable.First(g.Edges));
                }
            });
        }

        private void Compute<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
             where TEdge : IEdge<TVertex>
        {
            var dfs = new ConnectedComponentsAlgorithm<TVertex, TEdge>(g);
            dfs.Compute();
            if (g.VertexCount == 0)
            {
                Assert.Equal(0, dfs.ComponentCount);
                return;
            }

            Assert.True(0 < dfs.ComponentCount);
            Assert.True(dfs.ComponentCount <= g.VertexCount);
            foreach (var kv in dfs.Components)
            {
                Assert.True(0 <= kv.Value);
                Assert.True(kv.Value < dfs.ComponentCount);
            }

            foreach (var vertex in g.Vertices)
                foreach (var edge in g.AdjacentEdges(vertex))
                {
                    Assert.Equal(dfs.Components[edge.Source], dfs.Components[edge.Target]);
                }
        }
    }
}
