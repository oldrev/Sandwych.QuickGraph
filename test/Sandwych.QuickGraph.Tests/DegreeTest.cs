using System;
using System.Collections.Generic;
using System.Text;
using QuickGraph.Serialization;
using Xunit;

namespace QuickGraph.Tests
{
    public class DegreeTest
    {
        [Fact]
        public void DegreeSumEqualsTwiceEdgeCountAll()
        {
            foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
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

        [Fact]
        public void InDegreeSumEqualsEdgeCountAll()
        {
            foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
                this.InDegreeSumEqualsEdgeCount(g);
        }

        private void InDegreeSumEqualsEdgeCount<TVertex,TEdge>(
             IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.InDegree(v);

            Assert.Equal(edgeCount, degCount);
        }

        [Fact]
        public void OutDegreeSumEqualsEdgeCountAll()
        {
            foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
                this.OutDegreeSumEqualsEdgeCount(g);
        }

        private void OutDegreeSumEqualsEdgeCount<TVertex,TEdge>(
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
