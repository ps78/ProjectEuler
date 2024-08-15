using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NumberTheory;
using Xunit.Abstractions;
using System.Xml;

namespace UnitTests;

public class Graph_UnitTests(ITestOutputHelper output) : UnitTestBase(output)
{
    [Fact(DisplayName = "Default Constructor")]
    public void TestConstructor()
    {
        var g = new UndirectedGraph();

        g.EdgeCount.Should().Be(0);
        g.VertexCount.Should().Be(0);
        g.TotalWeight.Should().Be(0);
    }

    [Fact(DisplayName = "AddVertex")]
    public void TestAddVertex()
    {
        var g = new UndirectedGraph();

        g.AddVertex(new GraphVertex(1, 7)).Should().BeTrue();
        g.AddVertex(new GraphVertex(1, 8)).Should().BeFalse();

        g.EdgeCount.Should().Be(0);
        g.VertexCount.Should().Be(1);
        g.TotalWeight.Should().Be(7);
    }

    [Fact(DisplayName = "AddEdge")]
    public void TestAddEdge()
    {
        var g = new UndirectedGraph();
        var v1 = new GraphVertex(1, 3);
        var v2 = new GraphVertex(2, 7);
        var v3 = new GraphVertex(3, 11);

        g.AddEdge(new UndirectedGraphEdge(v1, v2, 0.1M)).Should().BeTrue();
        g.AddEdge(new UndirectedGraphEdge(v2, v1, 0.7M)).Should().BeFalse();
        g.AddEdge(new UndirectedGraphEdge(v1, v3, 0.3M)).Should().BeTrue();

        g.EdgeCount.Should().Be(2);
        g.VertexCount.Should().Be(3);
        g.TotalWeight.Should().Be(21.4M);
    }

    [Fact(DisplayName = "IsConnected")]
    public void TestIsConnected()
    {
        var g = new UndirectedGraph();
        var v1 = new GraphVertex(1);
        var v2 = new GraphVertex(2);
        var v3 = new GraphVertex(3);
        var v4 = new GraphVertex(4);

        g.AddEdge(new UndirectedGraphEdge(v1, v2));
        g.IsConnected.Should().BeTrue();

        g.AddEdge(new UndirectedGraphEdge(v3, v4));
        g.IsConnected.Should().BeFalse();

        g.AddEdge(new UndirectedGraphEdge(v2, v3));
        g.IsConnected.Should().BeTrue();
    }

    [Fact(DisplayName = "IsMinimallyConnected")]
    public void TestIsMinimallyConnected()
    {
        var g = new UndirectedGraph();
        var v1 = new GraphVertex(1);
        var v2 = new GraphVertex(2);
        var v3 = new GraphVertex(3);
        var v4 = new GraphVertex(4);

        g.AddEdge(new UndirectedGraphEdge(v1, v2));
        g.IsMinimallyConnected.Should().BeTrue();

        g.AddEdge(new UndirectedGraphEdge(v3, v4));
        g.IsMinimallyConnected.Should().BeFalse();

        g.AddEdge(new UndirectedGraphEdge(v2, v3));
        g.IsMinimallyConnected.Should().BeTrue();

        g.AddEdge(new UndirectedGraphEdge(v1, v4));
        g.IsMinimallyConnected.Should().BeFalse();
    }

    [Fact(DisplayName = "IsBridge")]
    public void TestIsBridgeEdge()
    {
        var g = new UndirectedGraph();
        var v1 = new GraphVertex(1);
        var v2 = new GraphVertex(2);
        var v3 = new GraphVertex(3);
        var v4 = new GraphVertex(4);

        var e12 = new UndirectedGraphEdge(v1, v2);
        var e23 = new UndirectedGraphEdge(v2, v3);
        var e34 = new UndirectedGraphEdge(v3, v4);
        var e41 = new UndirectedGraphEdge(v4, v1);
        var e13 = new UndirectedGraphEdge(v1, v3);
        var e24 = new UndirectedGraphEdge(v2, v4);

        g.AddEdge(e12);
        g.AddEdge(e23);
        g.AddEdge(e34);

        g.IsBridge(e12).Should().BeTrue();
        g.IsBridge(e23).Should().BeTrue();
        g.IsBridge(e34).Should().BeTrue();

        g.AddEdge(e41);

        g.IsBridge(e12).Should().BeFalse();
        g.IsBridge(e23).Should().BeFalse();
        g.IsBridge(e34).Should().BeFalse();
        g.IsBridge(e41).Should().BeFalse();
    }
}
