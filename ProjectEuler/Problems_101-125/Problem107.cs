using NumberTheory;
using System.Xml.Linq;

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
    // represents a symmetrical matrix with integer values, diagonal being zero
    private class GraphMatrix: ICloneable, IEquatable<GraphMatrix>
    {
        public readonly int N;

        // we only store the lower half of the matrix w/o diagonal, in linear form
        private readonly int[] Data; 
        
        public int this[int row, int col] {
            get 
            {
                if (row == col) return 0;
                if (row < col) (row, col) = (col, row);
                return Data[(row - 1) * row / 2 + col];
            }
            set
            {
                if (row == col) return;
                if (row < col) (row, col) = (col, row);
                Data[(row - 1) * row / 2 + col] = value;
            }
        }

        public int Cost => Data.Sum();

        public int EdgeCount => Data.Count(x => x != 0);

        public IEnumerable<(int From, int To, int Value)> Edges
        {
            get
            {
                for (int row = 0; row < N; row++) 
                    for (int col = 0; col < row; col++)
                    {
                        int value = this[row, col];
                        if (value != 0)
                            yield return (row, col, value);
                    }
            }
        }

        public IEnumerable<int> ConnectedNodes(int node)
        {
            for (int col = 0; col < N; col++)
                if (this[node, col] != 0)
                    yield return col;
        }

        public bool IsConnected
        {
            get
            {
                var foundNodes = new HashSet<int>();
                var currentNodes = new List<int>([0]);
                RecursiveExpand(foundNodes, currentNodes);
                return foundNodes.Count == N;
            }
        }

        /// <summary>
        /// True, if removing any of the edges will cause the network to break apart
        /// </summary>
        public bool IsMinimallyConnected
        {
            get
            {
                for (int i = 0; i < Data.Length; i++)
                    if (Data[i] != 0)
                    {
                        var clone = (GraphMatrix)Clone();
                        clone.Data[i] = 0;
                        if (clone.IsConnected)
                            return false;
                    }
                return true;
            }
        }

        public GraphMatrix(int n)
        {
            N = n;
            Data = new int[(N*N - N) / 2];
        }

        public object Clone()
        {
            GraphMatrix clone = new(N);
            Array.Copy(Data, clone.Data, Data.Length);
            return clone;
        }

        public static GraphMatrix FromFile(string file)
        {
            int n = -1;
            GraphMatrix? m = null;
            int row = 0;
            foreach (var line in File.ReadAllLines(file))
            {
                var values = line.Split(',').Select(s => s == "-" ? 0 : int.Parse(s)).ToArray();
                if (m == null)
                {
                    n = values.Length;
                    m = new GraphMatrix(n);
                }
                else if (values.Length != n)
                    throw new InvalidDataException($"The following line contains the wrong number of elements: {line}");

                if (row >= n)
                    throw new InvalidDataException($"The matrix contains too many rows");

                for (int col = 0; col < row; col++)
                    m[row, col] = values[col];
                row++;
            }
            if (m != null)
                return m;
            else
                throw new InvalidDataException("Could not read data");
        }

        private void RecursiveExpand(HashSet<int> foundNodes, List<int> currentNodes)
        {
            var currentCopy = new List<int>(currentNodes);
            foreach (int node in currentCopy)
            {
                foundNodes.Add(node);
                currentNodes.Remove(node);

                var newNodes = ConnectedNodes(node).Where(c => !foundNodes.Contains(c)).ToArray();

                if (newNodes.Any())
                {
                    currentNodes.AddRange(newNodes);
                    RecursiveExpand(foundNodes, currentNodes);
                }
            }
        }

        public override int GetHashCode()
        {
            int code = Data[0];
            for (int i = 1; i < Data.Length; i++)
                code ^= Data[i];
            return code;
        }

        public override bool Equals(object? obj)
        {
            if (obj is GraphMatrix m)
                return Equals(m);
            else
                return false;
        }

        public bool Equals(GraphMatrix? other)
        {
            if (other == null) 
                return false;
            
            if (N != other.N)
                return false;
            
            for (int i = 0; i < Data.Length; i++)
                if (Data[i] != other.Data[i])
                    return false;
            
            return true;
        }
    }

    public Problem107() : base(107, "Minimal Network", 0, 0) { }

    public override long Solve(long n)
    {
        var m = GraphMatrix.FromFile(Path.Combine(ResourcePath, "problem107.txt"));

        Console.WriteLine($"Loaded network with {m.N} vertices, {m.EdgeCount} edges and total cost of {m.Cost}");

        var astar = new AStarSearch<GraphMatrix>(Heuristic, IsGoal, Cost, GetNeighbors);
        var bestSolution = astar.Search([m]).First();
        
        Console.WriteLine($"Minimal cost: {bestSolution.Last().Cost}, Iteration count: {astar.IterationCount}");

        return 0;
    }

    /// <summary>
    /// Given a current network, the best possible soulution (lower boudn for cost) that could be 
    /// found is the network, where we keep for each node the lowest value edge
    /// </summary>
    private double Heuristic(GraphMatrix node)
    {
        double minPossibleCost = 0;
        for (int i = 0; i < node.N; i++)
        {
            int min = int.MaxValue;
            for (int j = 0; j < node.N; j++)
            {
                int edgeVal = node[i, j];
                if (edgeVal != 0 && edgeVal < min)
                    min = edgeVal;
            }
            minPossibleCost += min;
        }
        return minPossibleCost;// - node.Cost;
    }

    /// <summary>
    /// A network is in goal state, if no edge can be removed without 
    /// causing it to disconnect
    /// </summary>
    private bool IsGoal(GraphMatrix node) => node.IsMinimallyConnected;

    private double Cost(GraphMatrix node) => node.Cost;

    private List<(double CostToGetThere, GraphMatrix Neighbor)> GetNeighbors(GraphMatrix node)
    {
        var neighbors = new List<(double CostToGetThere, GraphMatrix Neighbor)>();
        var edges = node.Edges.ToList();
        foreach (var edge in edges.OrderByDescending(x => x.Value))
        {
            var neighbor = (GraphMatrix)node.Clone();

            neighbor[edge.From, edge.To] = 0;
            if (neighbor.IsConnected)
                neighbors.Add((-edge.Value, neighbor));
        }
        return neighbors;
    }
}
