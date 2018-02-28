using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QuickGraph.Serialization
{
    public static class TestGraphFactory
    {
        public static IEnumerable<string> GetFileNames()
        {
            var list = new List<string>();
            list.AddRange(Directory.GetFiles(Path.Combine(".", "graphml"), "g.*.graphml"));
            return list;
        }

        public static IEnumerable<BidirectionalGraph<string, Edge<string>>> GetBidirectionalGraphs()
        {
            yield return new BidirectionalGraph<string, Edge<string>>();
            foreach (var graphmlFile in TestGraphFactory.GetFileNames())
            {
                var g = LoadBidirectionalGraph(graphmlFile);
                yield return g;
            }
        }

        public static BidirectionalGraph<string, Edge<string>> LoadBidirectionalGraph(string graphmlFile)
        {
            TestConsole.WriteLine(graphmlFile);
            var g = new BidirectionalGraph<string, Edge<string>>();
            using (var reader = new StreamReader(graphmlFile))
            {
                g.DeserializeFromGraphML(
                    reader,
                    id => id,
                    (source, target, id) => new Edge<string>(source, target)
                    );
            }
            return g;
        }

        public static IEnumerable<AdjacencyGraph<string, Edge<string>>> GetAdjacencyGraphs()
        {
            yield return new AdjacencyGraph<string, Edge<string>>();
            foreach (var graphmlFile in TestGraphFactory.GetFileNames())
            {
                var g = LoadGraph(graphmlFile);
                yield return g;
            }
        }

        public static AdjacencyGraph<string, Edge<string>> LoadGraph(string graphmlFile)
        {
            TestConsole.WriteLine(graphmlFile);
            var g = new AdjacencyGraph<string, Edge<string>>();
            using (var reader = new StreamReader(graphmlFile))
            {
                g.DeserializeFromGraphML(
                    reader,
                    id => id,
                    (source, target, id) => new Edge<string>(source, target)
                    );
            }
            return g;
        }

        public static IEnumerable<UndirectedGraph<string, Edge<string>>> GetUndirectedGraphs()
        {
            yield return new UndirectedGraph<string, Edge<string>>();
            foreach (var g in GetAdjacencyGraphs())
            {
                var ug = new UndirectedGraph<string, Edge<string>>();
                ug.AddVerticesAndEdgeRange(g.Edges);
                yield return ug;
            }
        }

        public static UndirectedGraph<string, Edge<string>> LoadUndirectedGraph(string graphmlFile)
        {
            var g = LoadGraph(graphmlFile);
            var ug = new UndirectedGraph<string, Edge<string>>();
            ug.AddVerticesAndEdgeRange(g.Edges);
            return ug;
        }
    }
}
