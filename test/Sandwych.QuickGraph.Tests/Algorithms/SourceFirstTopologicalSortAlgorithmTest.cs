using System;
using QuickGraph.Serialization;
using QuickGraph.Algorithms.TopologicalSort;
using Xunit;

namespace QuickGraph.Algorithms
{
    public class SourceFirstTopologicalSortAlgorithmTest
    {
        [Fact]
        public void SortAll()
        {
            System.Threading.Tasks.Parallel.ForEach(TestGraphFactory.GetAdjacencyGraphs(), g =>
                this.Sort(g));
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
