using System;
using System.Collections.Generic;
using QuickGraph.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Algorithms.Search
{
    public class BidirectionalDepthFirstSearchAlgorithmTest
    {
        [Theory, GraphData(Type = GraphType.BidirectionalGraph)]
        public void ComputeAll(BidirectionalGraph<string, Edge<string>> g)
        {
            this.Compute(g);
        }

        private void Compute<TVertex, TEdge>(IBidirectionalGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var dfs = new BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge>(g);
            dfs.Compute();

            // let's make sure
            foreach (var v in g.Vertices)
            {
                Assert.True(dfs.VertexColors.ContainsKey(v));
                Assert.Equal(GraphColor.Black, dfs.VertexColors[v]);
            }
        }
    }
}
