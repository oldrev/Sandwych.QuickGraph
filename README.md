
# Sandwych.QuickGraph

Generic Graph Data Structures and Algorithms for .NET

Sandwych.QuickGraph is a port of QuickGraph to .NET Standard 1.6+.

QuickGraph provides generic directed/undirected graph datastructures and algorithms for .NET. QuickGraph comes with algorithms such as depth first seach, breath first search, A* search, shortest path, k-shortest path, maximum flow, minimum spanning tree, least common ancestors, etc... QuickGraph supports MSAGL, GLEE, and Graphviz to render the graphs, serialization to GraphML, etc...

QuickGraph is a portable library, i.e. supports .Net 4.0, Silverlight 4.0, Windows Phone 7, XBox 360 and Windows 8 Metro apps. QuickGraph is annotated with Code Contracts.

Other projects using QuickGraph:

* Reflector.Graph Addin
* Graph#, layout algorithms
* Jolt.Net, a backing store for a generic finite state machine implementation
* JSL StyleCop, Custom rules for Microsoft's StyleCop utility
* NDepend, codebase macro analysis
* Dependency Viewer

# NuGet Instructions

```powershell
PM> Install-Package Sandwych.QuickGraph.Core
```

# A simple example
This example takes a DataSet, builds the graph of table and constraints from the schema and computes the table topological sort (useful to figure order to populate a database):

```csharp
DataSet ds = new MyDataSet(); // your dataset
var graph = ds.ToGraph();  // wraps the dataset into a DataSetGraph
foreach(DataTable table in graph.TopologicalSort()) // applies a topological sort to the dataset graph
    Console.WriteLine(table.TableName); // in which order should we delete the tables?
```

# History

* Sandwych.QuickGraph 1.0.0 Ported to .NET Standard 1.6.
* QuickGraph 3.6. Portable Class Library support.
* QuickGraph 3.3.51106.0 available on nuget, no more support for .NET 2.0.
* QuickGraph 3.3 (updated) added Code Contracts reference assemblies
* QuickGraph 3.3 adds new graph data structures based on delegates
* QuickGraph 3.2 (bis) supporting Silveright
* QuickGraph 3.2 started using Code Contracts.
* QuickGraph 3.1 brings a Fibonacci Heap and support for 2.0 is back.
* QuickGraph 3.0 takes advantage of extension methods to simplify tasks.
* QuickGraph 2.0 introduced support for generic graph data structures
* The original QuickGraph for .net 1.0 was posted on CodeProject in 8 Dec 2003. It was time to do a refresh and make the graph generic.


The design of QuickGraph is inspired from the Boost Graph Library.

# Where to go next?

* Developer Manual
* FAQ
* Peli's blog
