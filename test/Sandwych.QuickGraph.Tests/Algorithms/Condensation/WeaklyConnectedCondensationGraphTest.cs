using System;
using System.IO;
using System.Collections.Generic;
using QuickGraph.Serialization;
using System.Threading.Tasks;
using QuickGraph;
using Xunit;

namespace QuickGraph.Algorithms.Condensation
{
    public class WeaklyConnectedCondensationGraphAlgorithmTest
    {
        [Theory, GraphData]
        public void WeaklyConnectedCondensatAll(AdjacencyGraph<string, Edge<string>> g)
        {
            this.WeaklyConnectedCondensate(g);
        }

        private void WeaklyConnectedCondensate<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var algo = new CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>(g);
            algo.StronglyConnected = false;
            algo.Compute();
            CheckVertexCount(g, algo);
            CheckEdgeCount(g, algo);
            CheckComponentCount(g, algo);
        }

        private void CheckVertexCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
            CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (var vertices in algo.CondensedGraph.Vertices)
                count += vertices.VertexCount;
            Assert.Equal(g.VertexCount, count);
        }

        private void CheckEdgeCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
            CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            int count = 0;
            foreach (var edges in algo.CondensedGraph.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in algo.CondensedGraph.Vertices)
                count += vertices.EdgeCount;
            Assert.Equal(g.EdgeCount, count);
        }


        private void CheckComponentCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
            CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of storngly connected components
            int components = g.WeaklyConnectedComponents<TVertex, TEdge>(new Dictionary<TVertex, int>());
            Assert.Equal(components, algo.CondensedGraph.VertexCount);
        }
    }
}
