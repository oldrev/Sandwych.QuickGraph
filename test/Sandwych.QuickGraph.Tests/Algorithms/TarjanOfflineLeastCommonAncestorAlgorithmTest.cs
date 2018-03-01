using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph.Collections;
using QuickGraph.Algorithms;
using QuickGraph.Serialization;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.Search;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Tests.Algorithms
{
    public class TarjanOfflineLeastCommonAncestorAlgorithmTest
    {
        [Theory, GraphData]
        public void TarjanOfflineLeastCommonAncestorAlgorithmAll(AdjacencyGraph<string, Edge<string>> g)
        {
            if (g.VertexCount == 0) return;

            var pairs = new List<SEquatableEdge<string>>();
            foreach (var v in g.Vertices)
                foreach (var w in g.Vertices)
                    if (!v.Equals(w))
                        pairs.Add(new SEquatableEdge<string>(v, w));

            int count = 0;
            foreach (var root in g.Vertices)
            {
                this.TarjanOfflineLeastCommonAncestorAlgorithm(
                    g,
                    root,
                    pairs.ToArray());
                if (count++ > 10) break;
            }
        }

        private void TarjanOfflineLeastCommonAncestorAlgorithm<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> g,
            TVertex root,
            SEquatableEdge<TVertex>[] pairs
            )
            where TEdge : IEdge<TVertex>
        {
            var lca = g.OfflineLeastCommonAncestorTarjan(root, pairs);
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(g);
            using (predecessors.Attach(dfs))
                dfs.Compute(root);

            TVertex ancestor;
            foreach (var pair in pairs)
                if (lca(pair, out ancestor))
                {
                    Assert.True(EdgeExtensions.IsPredecessor(predecessors.VertexPredecessors, root, pair.Source));
                    Assert.True(EdgeExtensions.IsPredecessor(predecessors.VertexPredecessors, root, pair.Target));
                }
        }
    }
}
