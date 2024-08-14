using NumberTheory;
using System.ComponentModel;
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
    /// <summary>
    /// Represents a symmetrical matrix with integer values, diagonal being zero
    /// There are 3 index types being used:
    /// 1) node index: from 0..N-1, an index per vertex
    /// 2) linear index: from 0..(N^2-N)/2-1, an index for each potential edge, lower part of symmetric matrix
    /// 3) (row,col): index-into the edge matrix, where row/col correspond to node indexes
    ///  
    /// </summary>
    private class GraphMatrix: IEquatable<GraphMatrix>
    {
        public readonly int N;
        public readonly int MaxEdgeCount; // (N*N-N)/2

        // we only store the lower half of the matrix w/o diagonal, in linear form
        private readonly int[] Data;

        // for each edge (same index as in data), this stores wheter it is a 
        // bridge edge (true) or not (false) or unknown (null)
        // A bridge edge is an edge that whose removal will split the graph
        private readonly bool?[] BridgeEdges;

        private bool? isConnected = null;
        private int? cost = null;
        private int? edgeCount = null;
        private int? hashCode = null;
        private bool? isMinimallyConnected = null;
        
        public int this[int row, int col]
        {
            get 
            {
                int idx = ToLinearIndex(row, col);
                return (idx == -1) ? 0 : Data[idx];
            }
        }

        public int Cost
        {
            get
            {
                cost ??= Data.Sum();
                return cost.Value;
            }
        }

        public int EdgeCount
        {
            get
            {
                edgeCount ??= Data.Count(x => x != 0);
                return edgeCount.Value;
            }
        }

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

        /// <summary>
        /// Returns all nodes that are connected to node
        /// </summary>
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
                if (isConnected == null)
                {
                    var foundNodes = new HashSet<int>();
                    var currentNodes = new List<int>([0]);
                    RecursiveExpand(foundNodes, currentNodes);
                    isConnected = foundNodes.Count == N;
                }
                return isConnected.Value;
            }
        }

        public bool IsBridgeEdge(int linearIndex)
        {
            if (!BridgeEdges[linearIndex].HasValue)
            {
                if (Data[linearIndex] == 0)
                {
                    BridgeEdges[linearIndex] = false;
                }
                else
                {
                    var clone = new GraphMatrix(this, linearIndex);
                    BridgeEdges[linearIndex] = !clone.IsConnected;
                }
            }

            return BridgeEdges[linearIndex].Value;
        }

        public bool IsBridgeEdge(int row, int col) => IsBridgeEdge(ToLinearIndex(row, col));

        /// <summary>
        /// True, if removing any of the edges will cause the network to break apart
        /// </summary>
        public bool IsMinimallyConnected
        {
            get
            {
                if (isMinimallyConnected == null)
                {
                    isMinimallyConnected = true;
                    for (int i = 0; i < Data.Length; i++)
                        if (Data[i] != 0 && !IsBridgeEdge(i))
                        {
                            isMinimallyConnected = false;
                            break;
                        }
                }
                return isMinimallyConnected.Value;
            }
        }

        public GraphMatrix(int n)
        {
            N = n;
            MaxEdgeCount = (N * N - N) / 2;
            Data = new int[MaxEdgeCount];
            BridgeEdges = new bool?[Data.Length];
        }

        public GraphMatrix(GraphMatrix other) : this(other.N)
        {
            Array.Copy(other.Data, Data, other.Data.Length);
            Array.Copy(other.BridgeEdges, BridgeEdges, other.BridgeEdges.Length);

            isConnected = other.isConnected;
            cost = other.cost;
            edgeCount = other.edgeCount;
            hashCode = other.hashCode;
            isMinimallyConnected = other.isMinimallyConnected;
        }

        public GraphMatrix(GraphMatrix other, int dropEdgeLinearIndex) : this(other.N)
        {
            Array.Copy(other.Data, Data, other.Data.Length);
            Data[dropEdgeLinearIndex] = 0;
        }

        public GraphMatrix(GraphMatrix other, (int row,int col) dropEdge) : this(other.N)
        {
            Array.Copy(other.Data, Data, other.Data.Length);
            Data[ToLinearIndex(dropEdge.row, dropEdge.col)] = 0;
        }

        public GraphMatrix(string file)
        {
            int[]? data = null;
            int row = 0;
            foreach (var line in File.ReadAllLines(file))
            {
                var values = line.Split(',').Select(s => s == "-" ? 0 : int.Parse(s)).ToArray();
                if (data == null)
                {
                    N = values.Length;
                    MaxEdgeCount = (N * N - N) / 2;
                    data = new int[MaxEdgeCount];
                }
                else if (values.Length != N)
                    throw new InvalidDataException($"The following line contains the wrong number of elements: {line}");

                if (row >= N)
                    throw new InvalidDataException($"The matrix contains too many rows");

                for (int col = 0; col < row; col++)
                    data[ToLinearIndex(row,col)] = values[col];
                row++;
            }
            if (data != null)
            {
                Data = data;
                BridgeEdges = new bool?[Data.Length];
            }
            else
                throw new InvalidDataException("Could not read data");
        }

        public int ToLinearIndex(int row, int col)
        {
            if (row == col) return -1;
            if (row < col)
                (row, col) = (col, row);
            return (row - 1) * row / 2 + col;
        }

        public (int row, int col) ToNodeIndex(int linearIndex)
        {
            for (int j = N-1; j >= 0; j--)
            {
                if (2*linearIndex >= j * j - j)
                {
                    return (j, (2 * linearIndex - j * j + j) / 2);
                }
            }
            throw new ArgumentException($"Linear index {linearIndex} is invalid");
        }

        public override int GetHashCode()
        {
            if (hashCode == null)
            {
                int code = Data[0];
                for (int i = 1; i < Data.Length; i++)
                    code ^= Data[i];
                hashCode = code;
            }
            return hashCode.Value;
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
            
            if (N != other.N || GetHashCode() != other.GetHashCode())
                return false;
            
            for (int i = 0; i < Data.Length; i++)
                if (Data[i] != other.Data[i])
                    return false;
            
            return true;
        }

        public override string ToString()
        {
            return $"Network with {N} nodes, {EdgeCount} edges, cost {Cost}";
        }

        /// <summary>
        /// Used by IsConnected to check if the network is connected or fragmented
        /// </summary>
        /// <param name="foundNodes"></param>
        /// <param name="currentNodes"></param>
        private void RecursiveExpand(HashSet<int> foundNodes, List<int> currentNodes)
        {
            var currentCopy = new List<int>(currentNodes);
            foreach (int node in currentCopy)
            {
                if (foundNodes.Count == N)
                    return;

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
    }

    public Problem107() : base(107, "Minimal Network", 0, 0) { }

    public override long Solve(long n)
    {
        var m = new GraphMatrix(Path.Combine(ResourcePath, "problem107_small.txt"));

        Console.WriteLine($"Loaded network with {m.N} vertices, {m.EdgeCount} edges and total cost of {m.Cost}");

        var astar = new AStarSearch<GraphMatrix>(Heuristic, IsGoal, Cost, GetNeighbors);
        var bestSolution = astar.Search([m]).First();
        
        Console.WriteLine($"Minimal cost: {bestSolution.Last().Cost}, Iteration count: {astar.IterationCount}");

        Console.WriteLine("Next solutions:");
        foreach (var sol in astar.Search([m]).Take(15))
        {
            Console.WriteLine($"Minimal cost: {sol.Last().Cost}");
        }

        return 0;
    }

    /// <summary>
    /// Given a current network, the best possible solution (lower bound for cost) that could be 
    /// found is the network, where we keep only the bridge edges
    /// The cost to get there is -Sum(bridge edges)
    /// </summary>
    private double Heuristic(GraphMatrix node)
    {
        // get all bridge edges. These must be kept for the goal state
        var edges = node.Edges
            .Where(e => node.IsBridgeEdge(e.From, e.To))
            .Select(x => (node.ToLinearIndex(x.From, x.To), x))
            .ToDictionary();

        // for all nodes, add the smallest edge from that node, 
        // unless it is already connected
        for (int i = 0; i < node.N; i++)
        {
            // if that node is already in the edges list, skip
            if (!edges.Any(x => x.Value.From == i || x.Value.To == i))
            {
                var minEdge = node.ConnectedNodes(i)
                    .Select(x => (From: i, To: x, Cost: node[i, x]))
                    .OrderBy(x => x.Cost)
                    .First();

                edges.Add(node.ToLinearIndex(minEdge.From, minEdge.To), minEdge);
            }
        }
        
        return edges.Sum(x => x.Value.Value) - (node.Cost);
    }

    /// <summary>
    /// A network is in goal state, if no edge can be removed without 
    /// causing it to disconnect
    /// </summary>
    private bool IsGoal(GraphMatrix node) => node.IsMinimallyConnected;

    /// <summary>
    /// nodes have no cost, we are only interested in minimizing the cost of the path, 
    /// i.e. removal of edges
    /// </summary>
    private double Cost(GraphMatrix node) => 0;

    private List<(double CostToGetThere, GraphMatrix Neighbor)> GetNeighbors(GraphMatrix node)
    {
        var neighbors = new List<(double CostToGetThere, GraphMatrix Neighbor)>();

        foreach (var (From, To, Value) in node.Edges.Where(e => !node.IsBridgeEdge(e.From, e.To)))
        {
            neighbors.Add((-Value, new GraphMatrix(node, (From, To))));
        }

        //return neighbors.OrderByDescending(n => n.CostToGetThere).ToList();
        return neighbors;
    }
}
