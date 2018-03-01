using System;
using System.Collections.Generic;
using QuickGraph.Serialization;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Algorithms.Search
{
    public class UndirectedDepthFirstAlgorithmSearchTest
    {
        private static bool IsDescendant<TVertex>(
            Dictionary<TVertex, TVertex> parents,
            TVertex u,
            TVertex v)
        {
            TVertex t;
            TVertex p = u;
            do
            {
                t = p;
                p = parents[t];
                if (p.Equals(v))
                    return true;
            }
            while (!t.Equals(p));

            return false;
        }

        [Theory, GraphData(Type = GraphType.UndirectedGraph)]
        public void UndirectedDepthFirstSearchAll(UndirectedGraph<string, Edge<string>> g)
        {
            this.UndirectedDepthFirstSearch(g);
        }

        private void UndirectedDepthFirstSearch<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var discoverTimes = new Dictionary<TVertex, int>();
            var finishTimes = new Dictionary<TVertex, int>();
            int time = 0;
            var dfs = new UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>(g);

            dfs.StartVertex += args =>
            {
                Assert.Equal(GraphColor.White, dfs.VertexColors[args]);
                Assert.False(parents.ContainsKey(args));
                parents[args] = args;
            };

            dfs.DiscoverVertex += args =>
            {
                Assert.Equal(GraphColor.Gray, dfs.VertexColors[args]);
                Assert.Equal(GraphColor.Gray, dfs.VertexColors[parents[args]]);

                discoverTimes[args] = time++;
            };

            dfs.ExamineEdge += (sender, args) =>
            {
                Assert.Equal(GraphColor.Gray, dfs.VertexColors[args.Source]);
            };

            dfs.TreeEdge += (sender, args) =>
            {
                var source = args.Source;
                var target = args.Target;
                Assert.Equal(GraphColor.White, dfs.VertexColors[target]);
                parents[target] = source;
            };

            dfs.BackEdge += (sender, args) =>
            {
                Assert.Equal(GraphColor.Gray, dfs.VertexColors[args.Target]);
            };

            dfs.ForwardOrCrossEdge += (sender, args) =>
            {
                Assert.Equal(GraphColor.Black, dfs.VertexColors[args.Target]);
            };

            dfs.FinishVertex += args =>
            {
                Assert.Equal(GraphColor.Black, dfs.VertexColors[args]);
                finishTimes[args] = time++;
            };

            dfs.Compute();

            // check
            // all vertices should be black
            foreach (var v in g.Vertices)
            {
                Assert.True(dfs.VertexColors.ContainsKey(v));
                Assert.Equal(GraphColor.Black, dfs.VertexColors[v]);
            }

            foreach (var u in g.Vertices)
            {
                foreach (var v in g.Vertices)
                {
                    if (!u.Equals(v))
                    {
                        Assert.True(
                            finishTimes[u] < discoverTimes[v]
                            || finishTimes[v] < discoverTimes[u]
                            || (
                            discoverTimes[v] < discoverTimes[u]
                            && finishTimes[u] < finishTimes[v]
                            && IsDescendant(parents, u, v)
                            )
                            || (
                            discoverTimes[u] < discoverTimes[v]
                            && finishTimes[v] < finishTimes[u]
                            && IsDescendant(parents, v, u)
                            )
                            );
                    }
                }
            }
        }
    }
}
