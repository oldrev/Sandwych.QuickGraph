using System;
using QuickGraph.Serialization;
using QuickGraph.Algorithms.TopologicalSort;
using Xunit;

namespace QuickGraph.Algorithms
{
    public class SourceFirstTopologicalSortAlgorithmTest
    {
        [Theory, GraphData]
        public void SortAll(AdjacencyGraph<string, Edge<string>> g)
        {
            this.Sort(g);
        }

        private void Sort<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var topo = new SourceFirstTopologicalSortAlgorithm<TVertex, TEdge>(g);
            try
            {
                topo.Compute();
            }
            catch (NonAcyclicGraphException)
            { }
        }
    }
}
