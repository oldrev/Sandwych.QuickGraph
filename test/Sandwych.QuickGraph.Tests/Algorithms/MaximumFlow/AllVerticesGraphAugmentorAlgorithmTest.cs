using System;
using QuickGraph.Serialization;
using QuickGraph.Collections;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Algorithms.MaximumFlow
{
    public class AllVerticesGraphAugmentorAlgorithmTest
    {
        [Fact]
        public void AugmentAll()
        {
            Parallel.ForEach(TestGraphFactory.GetAdjacencyGraphs(), g =>
                this.Augment(g));
        }

        private void Augment(
            IMutableVertexAndEdgeListGraph<string, Edge<string>> g)
        {
            int vertexCount = g.VertexCount;
            int edgeCount = g.EdgeCount;
            int vertexId = g.VertexCount + 1;
            int edgeID = g.EdgeCount + 1;
            using (var augmentor = new AllVerticesGraphAugmentorAlgorithm<string, Edge<string>>(
                g,
                () => (vertexId++).ToString(),
                (s, t) => new Edge<string>(s, t)
                ))
            {
                augmentor.Compute();
                VerifyCount(g, augmentor, vertexCount);
                VerifySourceConnector(g, augmentor);
                VerifySinkConnector(g, augmentor);
            }
            Assert.Equal(g.VertexCount, vertexCount);
            Assert.Equal(g.EdgeCount, edgeCount);
        }

        private static void VerifyCount<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            int vertexCount)
            where TEdge : IEdge<TVertex>
        {
            Assert.Equal(vertexCount + 2, g.VertexCount);
            Assert.True(g.ContainsVertex(augmentor.SuperSource));
            Assert.True(g.ContainsVertex(augmentor.SuperSink));
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                if (v.Equals(augmentor.SuperSource))
                    continue;
                if (v.Equals(augmentor.SuperSink))
                    continue;
                Assert.True(g.ContainsEdge(augmentor.SuperSource, v));
            }
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                if (v.Equals(augmentor.SuperSink))
                    continue;
                if (v.Equals(augmentor.SuperSink))
                    continue;
                Assert.True(g.ContainsEdge(v, augmentor.SuperSink));
            }
        }

    }

}
