using System;
using System.Collections.Generic;
using QuickGraph.Serialization;
using QuickGraph.Algorithms.TopologicalSort;

namespace QuickGraph.Algorithms
{
    public partial class UndirectedTopologicalSortAlgorithmTest
    {
        private void Compute(IUndirectedGraph<string, Edge<string>> g)
        {
            UndirectedTopologicalSortAlgorithm<string, Edge<string>> topo =
                new UndirectedTopologicalSortAlgorithm<string, Edge<string>>(g);
            topo.AllowCyclicGraph = true;
            topo.Compute();

            Display(topo);
        }

        private void Display(UndirectedTopologicalSortAlgorithm<string, Edge<string>> topo)
        {
            int index = 0;
            foreach (string v in topo.SortedVertices)
                TestConsole.WriteLine("{0}: {1}", index++, v);
        }
    }
}
