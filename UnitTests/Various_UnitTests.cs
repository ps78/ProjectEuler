using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Numerics;
using NumberTheory;
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;

namespace UnitTests;

public class Various_UnitTests(ITestOutputHelper output) : UnitTestBase(output)
{
    [Fact(DisplayName = "PolygonialNumbers")]
    public void TestPolygonalNumbers()
    {
        // the first 10 plus the 2 billionth element
        var n = new ulong[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1000000000 };
        var trigonalSeries = new ulong[] { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 500000000500000000};
        var squareSeries = new ulong[] { 1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 1000000000000000000 };
        var pentagonalSeries = new ulong[] { 1, 5, 12, 22, 35, 51, 70, 92, 117, 145, 1499999999500000000 };
        var hexagonalSeries = new ulong[] { 1, 6, 15, 28, 45, 66, 91, 120, 153, 190, 1999999999000000000 };
        var heptagonalSeries = new ulong[] { 1, 7, 18, 34, 55, 81, 112, 148, 189, 235, 2499999998500000000 };
        var octagonalSeries = new ulong[] { 1, 8, 21, 40, 65, 96, 133, 176, 225, 280, 2999999998000000000 };
        var nineteenSeries = new ulong[] { 1, 19, 54, 106, 175, 261, 364, 484, 621, 775, 8499999992500000000 };

        for (int i = 0; i < n.Length; i++)
        {
            PolygonalNumber.Triangle(n[i]).Should().Be(trigonalSeries[i]);
            PolygonalNumber.Square(n[i]).Should().Be(squareSeries[i]);
            PolygonalNumber.Pentagonal(n[i]).Should().Be(pentagonalSeries[i]);
            PolygonalNumber.Hexagonal(n[i]).Should().Be(hexagonalSeries[i]);
            PolygonalNumber.Heptagonal(n[i]).Should().Be(heptagonalSeries[i]);
            PolygonalNumber.Octagonal(n[i]).Should().Be(octagonalSeries[i]);
            PolygonalNumber.P(3, n[i]).Should().Be(trigonalSeries[i]);
            PolygonalNumber.P(4, n[i]).Should().Be(squareSeries[i]);
            PolygonalNumber.P(5, n[i]).Should().Be(pentagonalSeries[i]);
            PolygonalNumber.P(6, n[i]).Should().Be(hexagonalSeries[i]);
            PolygonalNumber.P(7, n[i]).Should().Be(heptagonalSeries[i]);
            PolygonalNumber.P(8, n[i]).Should().Be(octagonalSeries[i]);
            PolygonalNumber.P(19, n[i]).Should().Be(nineteenSeries[i]);
        }

        // Test the the correct subset is returned
        PolygonalNumber.P(19, 1, 10).Should().BeEquivalentTo(nineteenSeries.Take(10));
    }
}
