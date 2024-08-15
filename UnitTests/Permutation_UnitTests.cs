using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NumberTheory;
using Xunit.Abstractions;

namespace UnitTests;

public class Permutation_UnitTests(ITestOutputHelper output) : UnitTestBase(output)
{
    [Fact(DisplayName = "Permutation: Create")]
    public void TestCreate()
    {
        int[] set = [1, 2, 3];

        List<int[]> target = [[1, 2, 3], [1, 3, 2], [2, 1, 3], [2, 3, 1], [3, 1, 2], [3, 2, 1]];
        List<int[]> actual = Permutation.Create<int, int[]>(set).ToList();
        
        target.ToHashSet().Should().BeEquivalentTo(actual.ToHashSet());
    }
}
