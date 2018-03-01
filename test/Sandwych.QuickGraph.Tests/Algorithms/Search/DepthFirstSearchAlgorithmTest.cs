using System;
using System.Collections.Generic;
using QuickGraph.Serialization;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Xunit;

namespace QuickGraph.Algorithms.Search
{
    public class DepthFirstAlgorithmSearchTest
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

        [Theory, GraphData]
        public void DepthFirstSearchAll(AdjacencyGraph<string, Edge<string>> g)
        {
            this.DepthFirstSearch(g);
        }

        private void DepthFirstSearch<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var discoverTimes = new Dictionary<TVertex, int>();
            var finishTimes = new Dictionary<TVertex, int>();
            int time = 0;
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(g);

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

            dfs.ExamineEdge += args =>
            {
                Assert.Equal(GraphColor.Gray, dfs.VertexColors[args.Source]);
            };

            dfs.TreeEdge += args =>
            {
                Assert.Equal(GraphColor.White, dfs.VertexColors[args.Target]);
                parents[args.Target] = args.Source;
            };

            dfs.BackEdge += args =>
            {
                Assert.Equal(GraphColor.Gray, dfs.VertexColors[args.Target]);
            };

            dfs.ForwardOrCrossEdge += args =>
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
