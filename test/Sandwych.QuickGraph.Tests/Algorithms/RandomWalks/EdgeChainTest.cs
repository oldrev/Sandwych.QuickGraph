using System;
using System.Linq;
using System.Collections.Generic;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Algorithms.RandomWalks
{
    public class EdgeChainTest
    {
        [Fact]
        public void GenerateAll()
        {
            Parallel.ForEach(TestGraphFactory.GetAdjacencyGraphs(), g =>
                this.Generate(g));
        }

        private void Generate<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {

            foreach (var v in g.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(g);
                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis.Attach(walker))
                    walker.Generate(v);
            }
        }
    }
}
