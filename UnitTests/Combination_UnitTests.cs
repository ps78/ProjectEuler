using System.Collections.Generic;
using System.Linq;
using NumberTheory;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace UnitTests;

public class Combination_UnitTests(ITestOutputHelper output) : UnitTestBase(output)
{
    [Fact(DisplayName = "Combination: Count basic values")]
    public void Count_BasicValues()
    {
        Combination.Count(5, 2).Should().Be(10);
        Combination.Count(5, 3).Should().Be(10);
        Combination.Count(5, 0).Should().Be(1);
        Combination.Count(5, 5).Should().Be(1);
        Combination.Count(5, 6).Should().Be(0);
    }

    [Fact(DisplayName = "Combination: Create produces unique correct combinations")]
    public void Create_GeneratesCombinations()
    {
        var set = new[] { 1, 2, 3 };
        var expected = new List<int[]>
        {
            new[] { 1, 2 },
            new[] { 1, 3 },
            new[] { 2, 3 },
        };

        var actual = Combination.Create(set, 2).Select(x => x.ToArray()).ToList();

        // Compare ignoring order of the list entries
        expected.ToHashSet().Should().BeEquivalentTo(actual.ToHashSet());

        // Ensure no duplicates and count matches Count(n,k)
        actual.Count.Should().Be((int)Combination.Count(set.Length, 2));
        actual.Select(a => string.Join(",", a)).Distinct().Count().Should().Be(actual.Count);
    }

    [Fact(DisplayName = "Combination: CountWithRepetition basic values")]
    public void CountWithRepetition_BasicValues()
    {
        Combination.CountWithRepetition(3, 2).Should().Be(6); // C(3+2-1,2) = C(4,2) = 6
        Combination.CountWithRepetition(4, 2).Should().Be(10); // C(5,2) = 10
        Combination.CountWithRepetition(0, 0).Should().Be(1);
        Combination.CountWithRepetition(0, 2).Should().Be(0);
    }

    [Fact(DisplayName = "Combination: CreateWithRepetition generates correct combinations")]
    public void CreateWithRepetition_GeneratesCombinations()
    {
        var set = new[] { 'a', 'b', 'c' };
        var actual = Combination.CreateWithRepetition(set, 2).Select(x => new string(x.ToArray())).ToList();

        var expected = new List<string> { "aa", "ab", "ac", "bb", "bc", "cc" };

        expected.ToHashSet().Should().BeEquivalentTo(actual.ToHashSet());
        actual.Count.Should().Be((int)Combination.CountWithRepetition(set.Length, 2));
        actual.Distinct().Count().Should().Be(actual.Count);
    }

    [Fact(DisplayName = "Combination: Edge cases")]
    public void EdgeCases()
    {
        var emptySet = new int[0];

        // k > n -> no combinations
        Combination.Create(emptySet, 1).Any().Should().BeFalse();
        Combination.Create(emptySet, 0).Should().ContainSingle().Which.Should().BeEmpty();

        // CreateWithRepetition with k = 0 returns single empty result
        Combination.CreateWithRepetition(new[] { 1, 2 }, 0).Should().ContainSingle().Which.Should().BeEmpty();
    }

    [Theory(DisplayName = "Combination: Count small and edge values")]
    [InlineData(0, 0, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(1, 1, 1)]
    [InlineData(5, 1, 5)]
    [InlineData(5, 4, 5)]
    [InlineData(6, 3, 20)]
    [InlineData(10, 3, 120)]
    [InlineData(52, 5, 2_598_960)]
    [InlineData(5, -1, 0)]
    [InlineData(0, 1, 0)]
    public void Count_SmallAndEdgeValues(int n, int k, long expected)
    {
        Combination.Count(n, k).Should().Be(expected);
    }

    [Theory(DisplayName = "Combination: Count large values without overflow")]
    [InlineData(30, 15, 155_117_520L)]
    [InlineData(40, 20, 137_846_528_820L)]
    [InlineData(60, 30, 118_264_581_564_861_424L)]
    [InlineData(66, 33, 7_219_428_434_016_265_740L)]
    [InlineData(100, 3, 161_700L)]
    public void Count_LargeValues(int n, int k, long expected)
    {
        Combination.Count(n, k).Should().Be(expected);
    }

    [Fact(DisplayName = "Combination: Count throws when the result does not fit into a long")]
    public void Count_OverflowThrows()
    {
        FluentActions.Invoking(() => Combination.Count(68, 34)).Should().Throw<System.OverflowException>();
    }

    [Fact(DisplayName = "Combination: Count satisfies symmetry and Pascal's rule")]
    public void Count_PascalAndSymmetry()
    {
        for (int n = 1; n <= 50; n++)
            for (int k = 0; k <= n; k++)
            {
                Combination.Count(n, k).Should().Be(Combination.Count(n, n - k));
                if (k > 0)
                    Combination.Count(n, k).Should().Be(Combination.Count(n - 1, k - 1) + Combination.Count(n - 1, k));
            }
    }

    [Theory(DisplayName = "Combination: CountWithRepetition small and edge values")]
    [InlineData(1, 5, 1)]       // only "aaaaa"
    [InlineData(5, 1, 5)]
    [InlineData(2, 3, 4)]       // aaa, aab, abb, bbb
    [InlineData(3, 3, 10)]
    [InlineData(6, 3, 56)]      // three dice, order ignored
    [InlineData(10, 4, 715)]
    [InlineData(5, 0, 1)]
    [InlineData(0, 0, 1)]
    [InlineData(0, 3, 0)]
    [InlineData(3, -1, 0)]
    public void CountWithRepetition_SmallAndEdgeValues(int n, int k, long expected)
    {
        Combination.CountWithRepetition(n, k).Should().Be(expected);
    }

    [Fact(DisplayName = "Combination: Create matches brute-force subsets in lexicographic order")]
    public void Create_MatchesBruteForce()
    {
        for (int n = 0; n <= 8; n++)
        {
            var set = Enumerable.Range(10, n).ToArray();
            for (int k = 0; k <= n + 1; k++)
            {
                var expected = Enumerable.Range(0, 1 << n)
                    .Where(mask => System.Numerics.BitOperations.PopCount((uint)mask) == k)
                    .Select(mask => Enumerable.Range(0, n).Where(i => (mask & (1 << i)) != 0).Select(i => set[i]).ToArray())
                    .Select(c => string.Join(",", c))
                    .OrderBy(s => s, System.StringComparer.Ordinal)
                    .ToList();

                var actual = Combination.Create(set, k).Select(c => string.Join(",", c)).ToList();

                actual.Should().Equal(expected, $"n={n}, k={k}");
                actual.Count.Should().Be((int)Combination.Count(n, k));
            }
        }
    }

    [Fact(DisplayName = "Combination: Create returns independent lists and keeps element order")]
    public void Create_ReturnsIndependentLists()
    {
        var set = new[] { "x", "y", "z", "w" };
        var actual = Combination.Create(set, 3).ToList();

        actual.Should().HaveCount(4);
        actual[0].Should().Equal("x", "y", "z");
        actual[^1].Should().Equal("y", "z", "w");
        actual.Should().OnlyContain(c => c.Count == 3);
    }

    [Fact(DisplayName = "Combination: Create with negative k yields nothing")]
    public void Create_NegativeK()
    {
        Combination.Create(new[] { 1, 2, 3 }, -1).Should().BeEmpty();
    }

    [Fact(DisplayName = "Combination: CreateWithRepetition matches brute force")]
    public void CreateWithRepetition_MatchesBruteForce()
    {
        for (int n = 1; n <= 5; n++)
        {
            var set = Enumerable.Range(0, n).ToArray();
            for (int k = 0; k <= 4; k++)
            {
                // all n^k tuples, keep the non-decreasing ones
                var expected = new List<string>();
                int total = (int)System.Math.Pow(n, k);
                for (int t = 0; t < total; t++)
                {
                    var tuple = new int[k];
                    for (int i = k - 1, r = t; i >= 0; i--, r /= n)
                        tuple[i] = r % n;
                    if (tuple.Zip(tuple.Skip(1), (a, b) => a <= b).All(x => x))
                        expected.Add(string.Join(",", tuple));
                }

                var actual = Combination.CreateWithRepetition(set, k).Select(c => string.Join(",", c)).ToList();

                actual.Should().Equal(expected, $"n={n}, k={k}");
                actual.Count.Should().Be((int)Combination.CountWithRepetition(n, k));
            }
        }
    }

    [Fact(DisplayName = "Combination: CreateWithRepetition allows k larger than n")]
    public void CreateWithRepetition_KLargerThanN()
    {
        var actual = Combination.CreateWithRepetition(new[] { 'a', 'b' }, 3).Select(c => new string(c.ToArray())).ToList();
        actual.Should().Equal("aaa", "aab", "abb", "bbb");

        Combination.CreateWithRepetition(new[] { 7 }, 4).Should().ContainSingle().Which.Should().Equal(7, 7, 7, 7);
    }

    [Fact(DisplayName = "Combination: CreateWithRepetition edge cases")]
    public void CreateWithRepetition_EdgeCases()
    {
        Combination.CreateWithRepetition(new int[0], 0).Should().ContainSingle().Which.Should().BeEmpty();
        Combination.CreateWithRepetition(new int[0], 2).Should().BeEmpty();
        Combination.CreateWithRepetition(new[] { 1, 2 }, -1).Should().BeEmpty();
    }
}