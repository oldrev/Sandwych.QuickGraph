using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms;
using QuickGraph.Serialization;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.ShortestPath;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Tests.Algorithms.Search
{
    public class BestFirstFrontierSearchAlgorithmTest
    {
        [Fact]
        public void KrokFFig2Example()
        {
            var g = new BidirectionalGraph<char, SEquatableEdge<char>>();
            g.AddVerticesAndEdge(new SEquatableEdge<char>('A', 'C'));
            g.AddVerticesAndEdge(new SEquatableEdge<char>('A', 'B'));
            g.AddVerticesAndEdge(new SEquatableEdge<char>('B', 'E'));
            g.AddVerticesAndEdge(new SEquatableEdge<char>('B', 'D'));
            g.AddVerticesAndEdge(new SEquatableEdge<char>('E', 'F'));
            g.AddVerticesAndEdge(new SEquatableEdge<char>('E', 'G'));

            RunSearch(g);
        }

        [Theory, GraphData(Type = GraphType.BidirectionalGraph)]
        public void BestFirstFrontierSearchAllGraphs(BidirectionalGraph<string, Edge<string>> g)
        {
            RunSearch(g);
        }

        private void RunSearch<TVertex, TEdge>(IBidirectionalGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0) return;

            Func<TEdge, double> edgeWeights = e => 1;
            var distanceRelaxer = DistanceRelaxers.ShortestDistance;

            var search = new BestFirstFrontierSearchAlgorithm<TVertex, TEdge>(
                null,
                g,
                edgeWeights,
                distanceRelaxer);
            var root = Enumerable.First(g.Vertices);
            var target = Enumerable.Last(g.Vertices);
            var recorder = new VertexPredecessorRecorderObserver<TVertex, TEdge>();

            using (recorder.Attach(search))
                search.Compute(root, target);

            if (recorder.VertexPredecessors.ContainsKey(target))
            {
                TestConsole.WriteLine("cost: {0}", recorder.VertexPredecessors[target]);
                IEnumerable<TEdge> path;
                Assert.True(recorder.TryGetPath(target, out path));
            }
#if DEBUG
            TestConsole.WriteLine("operator max count: {0}", search.OperatorMaxCount);
#endif
        }

        [Theory, GraphData(Type = GraphType.BidirectionalGraph)]
        public void CompareBestFirstFrontierSearchAllGraphs(BidirectionalGraph<string, Edge<string>> g)
        {
            if (g.VertexCount == 0) return;

            var root = g.Vertices.First();
            foreach (var v in g.Vertices)
                if (!root.Equals(v))
                    CompareSearch(g, root, v);
        }

        private void CompareSearch<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> g,
            TVertex root, TVertex target)
            where TEdge : IEdge<TVertex>
        {
            Func<TEdge, double> edgeWeights = e => 1;
            var distanceRelaxer = DistanceRelaxers.ShortestDistance;

            var search = new BestFirstFrontierSearchAlgorithm<TVertex, TEdge>(
                null,
                g,
                edgeWeights,
                distanceRelaxer);
            var recorder = new VertexDistanceRecorderObserver<TVertex, TEdge>(edgeWeights);
            using (recorder.Attach(search))
                search.Compute(root, target);

            var dijkstra = new DijkstraShortestPathAlgorithm<TVertex, TEdge>(g, edgeWeights, distanceRelaxer);
            var dijRecorder = new VertexDistanceRecorderObserver<TVertex, TEdge>(edgeWeights);
            using (dijRecorder.Attach(dijkstra))
                dijkstra.Compute(root);

            var fvp = recorder.Distances;
            var dvp = dijRecorder.Distances;
            double cost;
            if (dvp.TryGetValue(target, out cost))
            {
                Assert.True(fvp.ContainsKey(target));
                Assert.Equal(dvp[target], fvp[target]);
            }
        }
    }
}
