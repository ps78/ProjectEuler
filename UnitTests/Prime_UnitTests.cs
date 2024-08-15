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

public class Prime_UnitTests(ITestOutputHelper output) : UnitTestBase(output)
{    
    [Fact(DisplayName = "MillerRabin.IsPrime()")]
    public void TestMillerRabinIsPrime()
    {
        var mrt = new MillerRabinTest();
        var tests = new Tuple<ulong, bool>[] {
            new Tuple<ulong, bool>(1, false),
            new Tuple<ulong, bool>(2, true),
            new Tuple<ulong, bool>(3, true),
            new Tuple<ulong, bool>(4, false),
            new Tuple<ulong, bool>(5, true),
            new Tuple<ulong, bool>(6, false),
            new Tuple<ulong, bool>(7, true),
            new Tuple<ulong, bool>(8, false),
            new Tuple<ulong, bool>(9, false),
            new Tuple<ulong, bool>(9, false),
            new Tuple<ulong, bool>(541, true),
            new Tuple<ulong, bool>(295927, false),
            new Tuple<ulong, bool>(2038074743L, true),
            new Tuple<ulong, bool>(2038074931L, true),
            new Tuple<ulong, bool>(4153749041212567733L, false),
            new Tuple<ulong, bool>(4153749041212567747L, true),
            new Tuple<ulong, bool>(10000004400000259, false),
            new Tuple<ulong, bool>(10000000000000061, true),
        };

        foreach (var t in tests)
            mrt.IsPrime(t.Item1).Should().Be(t.Item2);
    }

    [Fact(DisplayName = "MillerRabin performance tests")]
    public void TestMillerRabinIsPrimePerformance()
    {
        ulong prime = 10000000000000061;
        ulong pseudoPrime = 10000004400000259; // = 100000007 * 100000037
        ulong composite = 9999997200043387; // 9910800000043 * 1009;
        ulong nTestMR = 500;

        var mrt = new MillerRabinTest();

        var sw = Stopwatch.StartNew();
        double time = sw.Elapsed.TotalSeconds;
        testOutput.WriteLine("Building prime list: {0:n3} s", time);

        sw.Restart();
        for (ulong i = 0; i < nTestMR; i++)
            mrt.IsPrime(prime);
        time = (double)sw.ElapsedMilliseconds / nTestMR * 1000;
        testOutput.WriteLine("Miller Rabin test for large Primes: {0:n0} µs", time);

        sw.Restart();
        for (ulong i = 0; i < nTestMR; i++)
            mrt.IsPrime(pseudoPrime);
        time = (double)sw.ElapsedMilliseconds / nTestMR * 1000;
        testOutput.WriteLine("Miller Rabin test for large pseudo-prime: {0:n0} µs", time);

        sw.Restart();
        for (ulong i = 0; i < nTestMR; i++)
            mrt.IsPrime(composite);
        time = (double)sw.ElapsedMilliseconds / nTestMR * 1000;
        testOutput.WriteLine("Miller Rabin test for large composite: {0:n0} µs", time);
    }

    [Fact(DisplayName = "SieveOfEratosthenes.IsCountPrimes()")]
    public void TestSieveOfEratosthenesCountPrimes()
    {
        var s = new SieveOfEratosthenes(1000000);
        s.CountPrimes().Should().Be(78498);
        s.CountPrimes(3, 999982).Should().Be(78496);
        s.CountPrimes(1, 2).Should().Be(1);
        Assert.ThrowsAny<Exception>(() => s.CountPrimes(0, 1000001));
    }

    [Fact(DisplayName = "SieveOfEratosthenes.IsPrime()")]
    public void TestSieveOfEratosthenesIsPrime()
    {
        var s = new SieveOfEratosthenes(100);

        s.IsPrime(0).Should().BeFalse();
        s.IsPrime(1).Should().BeFalse();
        s.IsPrime(2).Should().BeTrue();
        s.IsPrime(95).Should().BeFalse();
        s.IsPrime(96).Should().BeFalse();
        s.IsPrime(97).Should().BeTrue();
        s.IsPrime(98).Should().BeFalse();
        s.IsPrime(99).Should().BeFalse();
        s.IsPrime(100).Should().BeFalse();
        s.IsPrime(9973).Should().BeTrue();
        s.IsPrime(9977).Should().BeFalse();
        s.IsPrime(10000).Should().BeFalse();
        s.IsPrime(100).Should().BeFalse();
        Assert.ThrowsAny<Exception>(() => s.IsPrime(10001));
    }

    [Fact(DisplayName = "SieveOfEratosthenes.GetPrimes()")]
    public void TestSieveOfEratosthenesGetPrimes()
    {
        var s = new SieveOfEratosthenes(100);
        var p = s.GetPrimes(20, 60);

        p.ConvertAll(x => (int)x).SequenceEqual(new[] { 23, 29, 31, 37, 41, 43, 47, 53, 59 }).Should().BeTrue();

        Assert.ThrowsAny<Exception>(() => s.GetPrimes(50, 200));
    }

    [Fact(DisplayName = "SieveOfEratosthenes performance tests")]
    public void TestSieveOfErastosthenesPerformance()
    {
        // prepare
        ulong sieveLimit = 100_000_037;
        ulong largePrime = 10_000_000_000_000_061;
        ulong largePseudoPrime = 10_000_004_400_000_259; // = 100000007 * 100000037
        ulong largeComposite = 9_999_997_200_043_387; // 9910800000043 * 1009;            
        ulong nTestSieveSmall = 1_000_000;
        ulong nTestSieveLarge = 3;
        Random rand = new Random();
        var smallTestNums = new ulong[nTestSieveSmall];
        for (int i = 0; i < (int)nTestSieveSmall; i++)
            smallTestNums[i] = (ulong)rand.Next(1, (int)sieveLimit);

        // build sieve
        var sw = Stopwatch.StartNew();
        var sieve = new SieveOfEratosthenes(sieveLimit);
        double time = sw.ElapsedMilliseconds;
        testOutput.WriteLine($"Building prime sive up to limit of {sieveLimit:N0}: {time:n0} ms");

        // retrieve prime list 
        sw.Restart();
        var primes = sieve.GetPrimes();
        time = sw.ElapsedMilliseconds;
        testOutput.WriteLine($"Time to retrieve list of all primes: {time:n0} ms");

        // count primes
        sw.Restart();
        var cnt = sieve.CountPrimes();
        time = sw.ElapsedMilliseconds;
        testOutput.WriteLine($"Time to count all {cnt:N0} primes: {time:n0} ms");

        // run isPrime tests
        sw.Restart();
        for (ulong i = 0; i < nTestSieveSmall; i++)
            sieve.IsPrime(smallTestNums[i]);
        time = nTestSieveSmall / sw.Elapsed.TotalSeconds;
        testOutput.WriteLine($"Sieve test for numbers < sieve limit:  {time:N0} / sec");
        
        sw.Restart();
        for (ulong i = 0; i < nTestSieveLarge; i++)
            sieve.IsPrime(largePrime);
        time = (double)sw.ElapsedMilliseconds / nTestSieveLarge;
        testOutput.WriteLine($"Sieve test for large prime > sieve limit:  {time:f2} ms");

        sw.Restart();
        for (ulong i = 0; i < nTestSieveLarge; i++)
            sieve.IsPrime(largePseudoPrime);
        time = (double)sw.ElapsedMilliseconds / nTestSieveLarge;
        testOutput.WriteLine($"Sieve test for large pseudo-prime > sieve limit: {time:f2} ms");

        sw.Restart();
        for (ulong i = 0; i < nTestSieveLarge; i++)
            sieve.IsPrime(largeComposite);
        time = (double)sw.ElapsedMilliseconds / nTestSieveLarge;
        testOutput.WriteLine($"Sieve test for large composite > sieve limit: {time:f2} ms");
    }

    [Fact(DisplayName = "SieveOfEratosthenes.GetPrimeFactors()")]
    public void TestGetPrimeFactors()
    {
        var sieve = new SieveOfEratosthenes(100000);

        // 15048 = 2^3 * 3^2 * 11 * 19
        var factors = sieve.GetPrimeFactors(15048).ToList(); factors.Sort();
        factors.Should().HaveCount(4);
        ((int)factors[0].Item1).Should().Be(2); ((int)factors[0].Item2).Should().Be(3);
        ((int)factors[1].Item1).Should().Be(3); ((int)factors[1].Item2).Should().Be(2);
        ((int)factors[2].Item1).Should().Be(11); ((int)factors[2].Item2).Should().Be(1);
        ((int)factors[3].Item1).Should().Be(19); ((int)factors[3].Item2).Should().Be(1);

        // 93 = 3*31
        factors = sieve.GetPrimeFactors(93).ToList(); factors.Sort();
        factors.Should().HaveCount(2);
        ((int)factors[0].Item1).Should().Be(3); ((int)factors[0].Item2).Should().Be(1);
        ((int)factors[1].Item1).Should().Be(31); ((int)factors[1].Item2).Should().Be(1);

        // 31 = 31
        factors = sieve.GetPrimeFactors(31).ToList();
        factors.Should().HaveCount(1);
        ((int)factors[0].Item1).Should().Be(31); ((int)factors[0].Item2).Should().Be(1);

        // 2620 = 2^2 * 5 * 131
        factors = sieve.GetPrimeFactors(2620).ToList();
        factors.Should().HaveCount(3);
        ((int)factors[0].Item1).Should().Be(2); ((int)factors[0].Item2).Should().Be(2);
        ((int)factors[1].Item1).Should().Be(5); ((int)factors[1].Item2).Should().Be(1);
        ((int)factors[2].Item1).Should().Be(131); ((int)factors[2].Item2).Should().Be(1);
    }

    [Fact(DisplayName = "SieveOfEratosthenes.GetFactors()")]
    public void TestGetFactors()
    {
        var sieve = new SieveOfEratosthenes(10000);
        List<ulong> factors;
        ulong[] expected;

        // test 2
        expected = new ulong[] { 1, 2 };
        factors = sieve.GetFactors(2).ToList(); factors.Sort();
        factors.Should().HaveCount(expected.Length);
        for (int i = 0; i < expected.Length; i++)
            factors[i].Should().Be(expected[i]);

        // test 11
        expected = new ulong[] { 1, 11 };
        factors = sieve.GetFactors(11).ToList(); factors.Sort();
        factors.Should().HaveCount(expected.Length);
        for (int i = 0; i < expected.Length; i++)
            factors[i].Should().Be(expected[i]);

        // test 284
        expected = new ulong[] { 1, 2, 4, 71, 142, 284 };
        factors = sieve.GetFactors(284).ToList(); factors.Sort();
        factors.Should().HaveCount(expected.Length);
        for (int i = 0; i < expected.Length; i++)
            factors[i].Should().Be(expected[i]);

        // test 504
        expected = new ulong[] { 1, 2, 3, 4, 6, 7, 8, 9, 12, 14, 18, 21, 24, 28, 36, 42, 56, 63, 72, 84, 126, 168, 252, 504 };
        factors = sieve.GetFactors(504).ToList(); factors.Sort();
        factors.Should().HaveCount(expected.Length);
        for (int i = 0; i < expected.Length; i++)
            factors[i].Should().Be(expected[i]);

        // test 2620
        expected = new ulong[] { 1, 2, 4, 5, 10, 20, 131, 262, 524, 655, 1310, 2620 };
        factors = sieve.GetFactors(2620).ToList(); factors.Sort();
        factors.Should().HaveCount(expected.Length);
        for (int i = 0; i < expected.Length; i++)
            factors[i].Should().Be(expected[i]);
    }

    [Theory(DisplayName = "CountPrimes.Get()")]
    [InlineData(1E1,  4)]
    [InlineData(1E2,  25)]
    [InlineData(1E3,  168)]
    [InlineData(1E4,  1229)]
    [InlineData(1E5,  9592)]
    [InlineData(1E6,  78498)]
    [InlineData(1E7,  664579)]
    [InlineData(1E8,  5761455)]
    [InlineData(1E9,  50847534)]
    [InlineData(1E10, 455052511)]
    [InlineData(1E11, 4118054813)]
    //[InlineData(1E12, 37607912018)]   // runs ~1 s
    //[InlineData(1E13, 346065536839)]  // runs ~5 s
    //[InlineData(1E14, 3204941750802)] // runs ~25 s
    public void TestCountPrimesGet(ulong n, ulong count)
    {
        CountPrimes.Get(n).Should().Be(count);
    }
}
