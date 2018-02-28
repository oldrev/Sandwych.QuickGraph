using System;
using Xunit;

namespace QuickGraph
{
    public static class EdgeListGraphTest
    {
        public static void Iteration<T,E>(IEdgeListGraph<T, E> g)
            where E : IEdge<T>
        {
            int n = g.EdgeCount;
            int i = 0;
            foreach (E e in g.Edges)
                ++i;
        }

        public static void Count<T,E>(IEdgeListGraph<T, E> g)
            where E : IEdge<T>
        {
            int n = g.EdgeCount;
            if (n == 0)
                Assert.True(g.IsEdgesEmpty);

            int i = 0;
            foreach (E e in g.Edges)
            {
                e.ToString();
                ++i;
            }
            Assert.Equal(n, i);
        }
    }
}
