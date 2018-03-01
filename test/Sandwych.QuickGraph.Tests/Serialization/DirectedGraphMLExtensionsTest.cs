using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph.Serialization;
using QuickGraph.Serialization.DirectedGraphML;
using System.Xml;
using QuickGraph.Algorithms;
using System.Diagnostics;
using Xunit;

namespace QuickGraph.Tests.Serialization
{
    public class DirectedGraphMLExtensionsTest
    {
        [Fact]
        public void SimpleGraph()
        {
            int[][] edges = { new int[]{ 1, 2, 3 },
                              new int[]{ 2, 3, 1 } };
            edges.ToAdjacencyGraph()
                .ToDirectedGraphML()
                .WriteXml("simple.dgml");

            if (Debugger.IsAttached)
            {
                Process.Start("simple.dgml");
            }

            edges.ToAdjacencyGraph()
                .ToDirectedGraphML()
                .WriteXml(Console.Out);
        }

        [Theory, GraphData]
        public void ToDirectedGraphML(AdjacencyGraph<string, Edge<string>> g)
        {
            var dg = g.ToDirectedGraphML();
            Assert.NotNull(g);
            Assert.Equal(dg.Nodes.Length, g.VertexCount);
            Assert.Equal(dg.Links.Length, g.EdgeCount);
        }
    }
}
