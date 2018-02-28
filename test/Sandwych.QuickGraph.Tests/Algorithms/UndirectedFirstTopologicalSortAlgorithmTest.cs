using System;
using System.Collections.Generic;
using QuickGraph.Serialization;
using QuickGraph.Algorithms.TopologicalSort;
using Xunit;

namespace QuickGraph.Algorithms
{
    public class UndirectedFirstTopologicalSortAlgorithmTest
    {
        [Fact]
        public void UndirectedFirstTopologicalSortAll()
        {
            foreach (var g in TestGraphFactory.GetUndirectedGraphs())
                this.Compute(g);
        }

        private void Compute<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var topo =
                new UndirectedFirstTopologicalSortAlgorithm<TVertex, TEdge>(g);
            topo.AllowCyclicGraph = true;
            topo.Compute();
        }

    }
}
