using System;
using System.Collections.Generic;
using Xunit;

namespace QuickGraph
{
    public partial class UndirectedGraphTest
    {
        public static void IsAdjacentEdgesEmpty<T,E>(IUndirectedGraph<T, E> g)
            where E : IEdge<T>
        {
            foreach (T v in g.Vertices)
            {
                Assert.Equal(
                    g.IsAdjacentEdgesEmpty(v),
                    g.AdjacentDegree(v) == 0);
            }
        }
    }
}
