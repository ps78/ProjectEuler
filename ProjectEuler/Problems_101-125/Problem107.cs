using NumberTheory;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=107
/// 
/// The following undirected network consists of seven vertices and twelve edges with a total weight of 243
/// (image)
/// The same network can be represented by the matrix below.
/// (matrix)
/// However, it is possible to optimise the network by removing some edges and still ensure that all points 
/// on the network remain connected. The network which achieves the maximum saving is shown below.
/// It has a weight of 93, representing a saving of 243 − 93 = 150 from the original network.
/// (image)
/// Using network.txt (right click and 'Save Link/Target As...'), a 6K text file containing a network 
/// with forty vertices, and given in matrix form, find the maximum saving which can be achieved 
/// by removing redundant edges whilst ensuring that the network remains connected.
/// 
/// </summary>
public class Problem107 : EulerProblemBase
{
    public Problem107() : base(107, "Minimal Network", 0, 259679) { }

    /// <summary>
    /// Solve using Kruskal's Algorithm: https://en.wikipedia.org/wiki/Kruskal%27s_algorithm
    /// </summary>
    public override long Solve(long n)
    {
        var (V, E) = Load(Path.Combine(ResourcePath, "problem107.txt"));

        var vertexSets = V.Select(v => (v, new HashSet<int>([v]))).ToDictionary();
        var minSpanningTree = new HashSet<(int from, int to, int weight)>();
        foreach (var edge in E.OrderBy(e => e.weight))
        {
            var fromSet = vertexSets[edge.from];
            var toSet = vertexSets[edge.to];
            if (fromSet != toSet)
            {
                minSpanningTree.Add(edge);
                var unionSet = fromSet.Union(toSet).ToHashSet();
                foreach (var v in unionSet)
                    vertexSets[v] = unionSet;
            }
        }

        return E.Sum(e => e.weight) - minSpanningTree.Sum(e => e.weight);
    }

    private (HashSet<int> Vertices, HashSet<(int from, int to, int weight)> Edges) Load(string file)
    {
        var lines = File.ReadAllLines(file);

        var Vertices = Enumerable.Range(0, lines.Length).ToHashSet();
        var Edges = new HashSet<(int from, int to, int weight)>();

        foreach (int fromVertex in Vertices)
        {
            var weights = lines[fromVertex].Split(',').Select(s => s == "-" ? 0 : int.Parse(s)).ToArray();
            for (int toVertex = fromVertex + 1; toVertex < Vertices.Count; toVertex++)
                if (weights[toVertex] != 0)
                    Edges.Add((fromVertex, toVertex, weights[toVertex]));
        }

        return (Vertices, Edges);
    }
}
