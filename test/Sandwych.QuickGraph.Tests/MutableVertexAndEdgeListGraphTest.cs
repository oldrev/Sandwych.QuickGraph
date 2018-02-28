using System;
using Xunit;

namespace QuickGraph
{
    public partial class MutableVertexAndEdgeListGraphTest
    {
        public void AddVertexOnly(IMutableVertexAndEdgeListGraph<string, Edge<string>> g, string v)
        {
            int vertexCount = g.VertexCount;
            g.AddVertex(v);
            Assert.Equal(vertexCount + 1, g.VertexCount);
            Assert.True(g.ContainsVertex(v));
            VerifyCounts(g);
        }

        public void AddAndRemoveVertex(IMutableVertexAndEdgeListGraph<int, Edge<int>> g, int v)
        {
            int vertexCount = g.VertexCount;
            g.AddVertex(v);
            Assert.Equal(vertexCount + 1, g.VertexCount);
            Assert.True(g.ContainsVertex(v));
            g.RemoveVertex(v);
            Assert.Equal(vertexCount, g.VertexCount);
            Assert.False(g.ContainsVertex(v));
            //VerifyCounts(g);
        }

        public void AddVertexAddEdgesAndRemoveTargetVertex(IMutableVertexAndEdgeListGraph<string, Edge<string>> g, string v1, string v2)
        {
            int vertexCount = g.VertexCount;
            int edgeCount = g.EdgeCount;

            g.AddVertex(v1);
            g.AddVertex(v2);
            Assert.Equal(vertexCount + 2, g.VertexCount);
            Assert.True(g.ContainsVertex(v1));
            Assert.True(g.ContainsVertex(v2));

            g.AddEdge(new Edge<string>(v1, v2));
            Assert.Equal(edgeCount + 1, g.EdgeCount);

            g.RemoveVertex(v2);
            Assert.Equal(vertexCount + 1, g.VertexCount);
            Assert.Equal(edgeCount, g.EdgeCount);
            Assert.True(g.ContainsVertex(v1));
            Assert.False(g.ContainsVertex(v2));
            VerifyCounts(g);
        }

        public void AddVertexAddEdgesAndRemoveSourceVertex(IMutableVertexAndEdgeListGraph<string, Edge<string>> g, string v1, string v2)
        {
            int vertexCount = g.VertexCount;
            int edgeCount = g.EdgeCount;

            g.AddVertex(v1);
            g.AddVertex(v2);
            Assert.Equal(vertexCount + 2, g.VertexCount);
            Assert.True(g.ContainsVertex(v1));
            Assert.True(g.ContainsVertex(v2));

            g.AddEdge(new Edge<string>(v1, v2));
            Assert.Equal(edgeCount + 1, g.EdgeCount);

            g.RemoveVertex(v1);
            Assert.Equal(vertexCount + 1, g.VertexCount);
            Assert.Equal(edgeCount, g.EdgeCount);
            Assert.True(g.ContainsVertex(v2));
            Assert.False(g.ContainsVertex(v1));
            VerifyCounts(g);
        }

        private void VerifyCounts(IMutableVertexAndEdgeListGraph<string, Edge<string>> g)
        {
            int i = 0;
            foreach (string v in g.Vertices)
                i++;
            Assert.Equal(g.VertexCount, i);

            i = 0;
            foreach (string v in g.Vertices)
                foreach (Edge<string> e in g.OutEdges(v))
                    i++;
            Assert.Equal(g.EdgeCount, i);

            i = 0;
            foreach (Edge<string> e in g.Edges)
                i++;
            Assert.Equal(g.EdgeCount, i);
        }
    }
}
