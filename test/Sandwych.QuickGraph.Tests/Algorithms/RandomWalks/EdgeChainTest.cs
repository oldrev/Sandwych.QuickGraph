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
        [Theory, GraphData]
        public void GenerateAll(AdjacencyGraph<string, Edge<string>> g)
        {
            this.Generate(g);
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
