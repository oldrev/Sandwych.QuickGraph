using System;
using System.IO;
using System.Collections.Generic;
using QuickGraph.Algorithms.Condensation;
using QuickGraph.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Algorithms.Condensation
{
    public class StronglyConnectedCondensationGraphAlgorithmTest
    {
        [Theory, GraphData]
        public void StronglyConnectedCondensateAll(AdjacencyGraph<string, Edge<string>> g)
        {
            this.StronglyConnectedCondensate(g);
        }

        private void StronglyConnectedCondensate<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var cg = g.CondensateStronglyConnected<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>();

            CheckVertexCount(g, cg);
            CheckEdgeCount(g, cg);
            CheckComponentCount(g, cg);
            CheckDAG(g, cg);
        }

        private void CheckVertexCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (var vertices in cg.Vertices)
                count += vertices.VertexCount;
            Assert.Equal(g.VertexCount, count);
        }

        private void CheckEdgeCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            int count = 0;
            foreach (var edges in cg.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in cg.Vertices)
                count += vertices.EdgeCount;
            Assert.Equal(g.EdgeCount, count);
        }


        private void CheckComponentCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of storngly connected components
            IDictionary<TVertex, int> components;
            int componentCount = g.StronglyConnectedComponents(out components);
            Assert.Equal(componentCount, cg.VertexCount);
        }

        private void CheckDAG<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check it's a dag
            try
            {
                cg.TopologicalSort();
            }
            catch (NonAcyclicGraphException)
            {
                Assert.False(true);
            }

        }
    }
}
