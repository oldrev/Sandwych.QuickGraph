using System;
using System.Collections.Generic;
using QuickGraph.Serialization;
using QuickGraph.Algorithms.ConnectedComponents;
using Xunit;

namespace QuickGraph.Algorithms
{
    public partial class StronglyConnectedComponentAlgorithmTest
    {
        [Fact]
        public void EmptyGraph()
        {
            var g = new AdjacencyGraph<string, Edge<string>>(true);
            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<String>>(g);
            strong.Compute();
            Assert.Equal(0, strong.ComponentCount);
            checkStrong(strong);
        }

        [Fact]
        public void OneVertex()
        {
            AdjacencyGraph<string, Edge<string>> g = new AdjacencyGraph<string, Edge<string>>(true);
            g.AddVertex("test");
            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<String>>(g);
            strong.Compute();
            Assert.Equal(1, strong.ComponentCount);

            checkStrong(strong);
        }

        [Fact]
        public void TwoVertex()
        {
            AdjacencyGraph<string, Edge<string>> g = new AdjacencyGraph<string, Edge<string>>(true);
            g.AddVertex("v1");
            g.AddVertex("v2");
            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<String>>(g);
            strong.Compute();
            Assert.Equal(2, strong.ComponentCount);

            checkStrong(strong);
        }

        [Fact]
        public void TwoVertexOnEdge()
        {
            AdjacencyGraph<string, Edge<string>> g = new AdjacencyGraph<string, Edge<string>>(true);
            g.AddVertex("v1");
            g.AddVertex("v2");
            g.AddEdge(new Edge<string>("v1", "v2"));
            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<String>>(g);
            strong.Compute();
            Assert.Equal(2, strong.ComponentCount);

            checkStrong(strong);
        }


        [Fact]
        public void TwoVertexCycle()
        {
            AdjacencyGraph<string, Edge<string>> g = new AdjacencyGraph<string, Edge<string>>(true);
            g.AddVertex("v1");
            g.AddVertex("v2");
            g.AddEdge(new Edge<string>("v1", "v2"));
            g.AddEdge(new Edge<string>("v2", "v1"));
            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<String>>(g);
            strong.Compute();
            Assert.Equal(1, strong.ComponentCount);

            checkStrong(strong);
        }

        [Theory, GraphData]
        public void StronglyConnectedComponentAll(AdjacencyGraph<string, Edge<string>> g)
        {
            this.Compute(g);
        }

        private void Compute<TVertex, TEdge>(AdjacencyGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var strong = new StronglyConnectedComponentsAlgorithm<TVertex, TEdge>(g);

            strong.Compute();
            checkStrong(strong);
        }

        private void checkStrong<TVertex, TEdge>(StronglyConnectedComponentsAlgorithm<TVertex, TEdge> strong)
            where TEdge : IEdge<TVertex>
        {
            Assert.Equal(strong.VisitedGraph.VertexCount, strong.Components.Count);
            Assert.Equal(strong.VisitedGraph.VertexCount, strong.DiscoverTimes.Count);
            Assert.Equal(strong.VisitedGraph.VertexCount, strong.Roots.Count);

            foreach (var v in strong.VisitedGraph.Vertices)
            {
                Assert.True(strong.Components.ContainsKey(v));
                Assert.True(strong.DiscoverTimes.ContainsKey(v));
            }

            foreach (var de in strong.Components)
            {
                //Assert.NotNull(de.Key);
                Assert.True(de.Value <= strong.ComponentCount);
            }

            /*
            foreach (var de in strong.DiscoverTimes)
            {
                Assert.NotNull(de.Key);
            }
            */
        }
    }
}
