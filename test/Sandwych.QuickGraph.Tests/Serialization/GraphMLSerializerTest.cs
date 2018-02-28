using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using QuickGraph.Collections;
using Xunit;

namespace QuickGraph.Serialization
{
    public class GraphMLSerializerIntegrationTest
    {
        [Fact]
        public void DeserializeFromGraphMLNorth()
        {
            foreach (var graphmlFile in TestGraphFactory.GetFileNames())
            {
                Console.Write(graphmlFile);
                var g = new AdjacencyGraph<string, Edge<string>>();
                using (var reader = new StreamReader(graphmlFile))
                {
                    g.DeserializeFromGraphML(
                        reader,
                        id => id,
                        (source, target, id) => new Edge<string>(source, target)
                        );
                }
                Console.Write(": {0} vertices, {1} edges", g.VertexCount, g.EdgeCount);

                var vertices = new Dictionary<string, string>();
                foreach (var v in g.Vertices)
                    vertices.Add(v, v);

                // check all nodes are loaded
                var settings = new XmlReaderSettings();
                settings.XmlResolver = new GraphMLXmlResolver();
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
                using (var xreader = XmlReader.Create(graphmlFile, settings))
                {
                    var doc = new XPathDocument(xreader);
                    foreach (XPathNavigator node in doc.CreateNavigator().Select("/graphml/graph/node"))
                    {
                        string id = node.GetAttribute("id", "");
                        Assert.True(vertices.ContainsKey(id));
                    }
                    TestConsole.Write(", vertices ok");

                    // check all edges are loaded
                    foreach (XPathNavigator node in doc.CreateNavigator().Select("/graphml/graph/edge"))
                    {
                        string source = node.GetAttribute("source", "");
                        string target = node.GetAttribute("target", "");
                        Assert.True(g.ContainsEdge(vertices[source], vertices[target]));
                    }
                    TestConsole.Write(", edges ok");
                }
                TestConsole.WriteLine();
            }
        }
    }


}
