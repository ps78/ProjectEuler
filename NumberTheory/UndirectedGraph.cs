using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NumberTheory;

/// <summary>
/// Vertex in a graph. Note that Id is supposed to be unique and two
/// vertices with the same id are considered equal, regardless of the weight
/// </summary>
public class GraphVertex(int Id, decimal Weight = 0) : IEquatable<GraphVertex>
{
    public readonly int Id = Id;
    public readonly decimal Weight = Weight;
    public bool Equals(GraphVertex? other) => other != null && other.Id == Id;
    public override bool Equals(object? obj) => (obj is GraphVertex v) ? Equals(v) : false;
    public override string ToString() => Weight == 0 ? $"{Id}" : $"{Id}[{Weight}]";
    public override int GetHashCode() => Id.GetHashCode();
}

/// <summary>
/// Note that two edges with identical start and end vertex are considered equal
/// regardless of the weight
/// The constructor makes sure that always From.Id <= To.Id
/// </summary>
public class UndirectedGraphEdge: IEquatable<UndirectedGraphEdge>
{
    public readonly GraphVertex From;
    public readonly GraphVertex To;
    public readonly decimal Weight;
    public (int,int) Id => (From.Id, To.Id);
    public UndirectedGraphEdge(GraphVertex from, GraphVertex to, decimal weight = 0)
    {
        (From, To) = from.Id <= to.Id ? (from, to) : (to, from);
        Weight = weight;
    }
    public bool Equals(UndirectedGraphEdge? other) => other != null && other.From.Id == From.Id && other.To.Id == To.Id;
    public override bool Equals(object? obj) => (obj is UndirectedGraphEdge e) ? Equals(e) : false;
    public override string ToString() => $"({From}-{To})" + (Weight == 0 ? "" : $"[{Weight}])");
    public override int GetHashCode() => From.GetHashCode() ^ To.GetHashCode();
}

/// <summary>
/// Represents a graph with vertices and edges, where the edges are undirected
/// </summary>
public class UndirectedGraph : IEquatable<UndirectedGraph>
{
    #region Fields

    protected readonly HashSet<GraphVertex> vertices = [];
    
    protected readonly HashSet<UndirectedGraphEdge> edges = [];
    
    /// <summary>
    /// Cache for the IsBridge() method
    /// </summary>
    private readonly Dictionary<UndirectedGraphEdge, bool?> isBridge = [];
    
    /// <summary>
    /// Cache for the IsConnected property
    /// </summary>
    private bool? isConnected = false;

    /// <summary>
    /// Cache for the IsMinimallyConnected property
    /// </summary>
    private bool? isMinimallyConnected = false;

    #endregion
    #region Properties

    public IReadOnlyCollection<GraphVertex> Vertices => vertices;
    
    public IReadOnlyCollection<UndirectedGraphEdge> Edges => edges;
    
    public int EdgeCount => edges.Count;
    
    public int VertexCount => vertices.Count;
    
    public bool Empty => VertexCount == 0;
    
    public decimal TotalVertexWeight => vertices.Sum(v => v.Weight);
    
    public decimal TotalEdgeWeight => edges.Sum(e => e.Weight);

    public decimal TotalWeight => TotalVertexWeight + TotalEdgeWeight;

    /// <summary>
    /// Returns true if all vertexes are directly or indirectly connected to all
    /// other vertices through edges.
    /// Computation is fairly expensive but the result is cached
    /// </summary>
    public bool IsConnected
    {
        get
        {
            if (Empty)
                isConnected = false;
            
            if (!isConnected.HasValue)
            {
                var currentVertices = new HashSet<GraphVertex>();
                var foundVertices = new HashSet<GraphVertex>();
                var newVertices = new HashSet<GraphVertex>([vertices.First()]);

                while (newVertices.Count > 0)
                {
                    foreach (var item in newVertices)
                        foundVertices.Add(item);

                    currentVertices = new HashSet<GraphVertex>(newVertices);

                    newVertices.Clear();

                    foreach (var vertex in currentVertices.SelectMany(v => ConnectedVertices(v)))
                        if (!foundVertices.Contains(vertex))
                            newVertices.Add(vertex);
                }
                
                isConnected = foundVertices.Count == VertexCount;
            }
            return isConnected.Value;
        }
    }

    /// <summary>
    /// Is true if all edges are bridge edges
    /// </summary>
    public bool IsMinimallyConnected
    {
        get
        {
            if (Empty || !IsConnected)
                isMinimallyConnected = false;

            if (!isMinimallyConnected.HasValue)
            {
                isMinimallyConnected = edges.All(e => IsBridge(e));
            }
            return isMinimallyConnected.Value;
        }
    }

    #endregion
    #region Public Methods

    /// <summary>
    /// Creates an empty graph
    /// </summary>
    public UndirectedGraph() { }

    /// <summary>
    /// Copy constructor
    /// </summary>
    public UndirectedGraph(UndirectedGraph other)
    {
        this.vertices = new HashSet<GraphVertex>(other.vertices);
        this.edges = new HashSet<UndirectedGraphEdge>(other.edges);
        this.isBridge = new Dictionary<UndirectedGraphEdge, bool?>(other.isBridge);
        this.isConnected = other.isConnected;
    }

    /// <summary>
    /// Adds a vertex. 
    /// Returns false if it was already part of the graph
    /// </summary>
    public bool AddVertex(GraphVertex vertex)
    {
        if (vertices.Add(vertex))
        {
            InvalidateConnected();
            isConnected = false; // we know this for sure, so set it
            return true;
        }
        else
            return false;
    }
    
    /// <summary>
    /// Will also add vertices if they are not part of the graph yet.
    /// Returns false if the edge was already part of the graph
    /// </summary>
    public bool AddEdge(UndirectedGraphEdge edge)
    {
        if (edges.Add(edge))
        {
            AddVertex(edge.From);
            AddVertex(edge.To);
            isBridge[edge] = null;
            InvalidateConnected();
            return true;
        }
        else
            return false;
    }
    
    public bool RemoveEdge(UndirectedGraphEdge edge)
    {
        if (edges.Remove(edge))
        {
            isBridge.Remove(edge);
            InvalidateConnected();
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Returns all edges connecting the given vertex
    /// </summary>
    public IEnumerable<UndirectedGraphEdge> VertexEdges(GraphVertex vertex)
    {
        return edges.Where(e => e.From.Equals(vertex) || e.To.Equals(vertex));
    }

    /// <summary>
    /// All vertices connected by one edge directly to the given vertex
    /// </summary>
    public IEnumerable<GraphVertex> ConnectedVertices(GraphVertex vertex)
    {
        foreach (var e in edges)
        {
            if (e.From.Equals(vertex))
                yield return e.To;
            else if (e.To.Equals(vertex))
                yield return e.From;
        }
    }

    /// <summary>
    /// Returns the degree of the vertex, i.e. the number of edges 
    /// from/to the vertex
    /// </summary>
    public int Degree(GraphVertex vertex) => VertexEdges(vertex).Count();

    /// <summary>
    /// Verifies if the given edge is a bridge, i.e. if by removing it the 
    /// network would become disconnected.
    /// Computation is expensive, but property is cached
    /// </summary>
    public bool IsBridge(UndirectedGraphEdge edge)
    {
        if (isBridge.TryGetValue(edge, out bool? result))
        {
            if (!result.HasValue)
            {
                var copy = new UndirectedGraph(this);
                copy.RemoveEdge(edge);
                result = !copy.IsConnected;
                isBridge[edge] = result;
            }
            return result.Value;
        }
        else
            throw new ArgumentException("Given edge is not part of the network");
    }

    public override string ToString()
    {
        return $"Graph ({VertexCount} vertices, {EdgeCount} edge, Weight {TotalWeight})";
    }

    public override int GetHashCode()
    {
        int code = 0;
        foreach (var e in edges)
            code ^= e.GetHashCode();
        code ^= VertexCount.GetHashCode();
        return code;
    }

    public bool Equals(UndirectedGraph? other)
    {
        if (other == null) 
            return false;

        if (EdgeCount != other.EdgeCount || VertexCount != other.VertexCount)
            return false;

        return edges.SetEquals(other.edges) && vertices.SetEquals(other.vertices);
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    #endregion
    #region Protected Methods

    protected void InvalidateConnected()
    {
        isConnected = null;
        isMinimallyConnected = null;
        foreach (var e in isBridge.Keys)
            isBridge[e] = null;
    }

    #endregion
}
