using System;
using System.Collections.Generic;
using QuickGraph.Serialization;
using QuickGraph.Collections;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Algorithms
{
    public class EulerianTrailAlgorithmTest
    {
        [Theory]
        [GraphData(FileName = "g.80.6.graphml")]
        [GraphData(FileName = "g.81.16.graphml")]
        public void EulerianTrailAll(AdjacencyGraph<string, Edge<string>> g)
        {
            this.ComputeTrail(g, (s, t) => new Edge<string>(s, t));
        }

        private void ComputeTrail<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            Func<TVertex, TVertex, TEdge> edgeCreator)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0)
                return;

            int oddCount = 0;
            foreach (var v in g.Vertices)
                if (g.OutDegree(v) % 2 == 0)
                    oddCount++;

            int circuitCount = EulerianTrailAlgorithm<TVertex, TEdge>.ComputeEulerianPathCount(g);
            if (circuitCount == 0)
                return;

            var trail = new EulerianTrailAlgorithm<TVertex, TEdge>(g);
            trail.AddTemporaryEdges((s, t) => edgeCreator(s, t));
            trail.Compute();
            var trails = trail.Trails();
            trail.RemoveTemporaryEdges();

            //TestConsole.WriteLine("trails: {0}", trails.Count);
            //int index = 0;
            //foreach (var t in trails)
            //{
            //    TestConsole.WriteLine("trail {0}", index++);
            //    foreach (Edge<string> edge in t)
            //        TestConsole.WriteLine("\t{0}", t);
            //}

            // lets make sure all the edges are in the trail
            var edgeColors = new Dictionary<TEdge, GraphColor>(g.EdgeCount);
            foreach (var edge in g.Edges)
                edgeColors.Add(edge, GraphColor.White);
            foreach (var t in trails)
                foreach (var edge in t)
                    Assert.True(edgeColors.ContainsKey(edge));

        }
    }
}
