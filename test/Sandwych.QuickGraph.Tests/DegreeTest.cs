using System;
using System.Collections.Generic;
using System.Text;
using QuickGraph.Serialization;
using Xunit;

namespace QuickGraph.Tests
{
    public class DegreeTest
    {
        [Theory, GraphData(Type = GraphType.BidirectionalGraph)]
        public void DegreeSumEqualsTwiceEdgeCountAll(BidirectionalGraph<string, Edge<string>> g)
        {
            this.DegreeSumEqualsTwiceEdgeCount(g);
        }

        private void DegreeSumEqualsTwiceEdgeCount<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.Degree(v);

            Assert.Equal(edgeCount * 2, degCount);
        }

        [Theory, GraphData(Type = GraphType.BidirectionalGraph)]
        public void InDegreeSumEqualsEdgeCountAll(BidirectionalGraph<string, Edge<string>> g)
        {
            this.InDegreeSumEqualsEdgeCount(g);
        }

        private void InDegreeSumEqualsEdgeCount<TVertex, TEdge>(
             IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.InDegree(v);

            Assert.Equal(edgeCount, degCount);
        }

        [Theory, GraphData(Type = GraphType.BidirectionalGraph)]
        public void OutDegreeSumEqualsEdgeCountAll(BidirectionalGraph<string, Edge<string>> g)
        {
            this.OutDegreeSumEqualsEdgeCount(g);
        }

        private void OutDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.OutDegree(v);

            Assert.Equal(edgeCount, degCount);
        }

    }
}
