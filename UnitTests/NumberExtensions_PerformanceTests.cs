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

public class NumberExtensions_PerformanceTests
{
    #region Setup / Helper

    private readonly ITestOutputHelper testOutput;

    public NumberExtensions_PerformanceTests(ITestOutputHelper testOutputHelper)
    {
        testOutput = testOutputHelper;
    }
    #endregion
    #region Test Methods
    
    [Fact(DisplayName = "Performance: Power() / BigPower")]
    public void TestPowerPerformance()
    {
        var r = new Random();
        int nTest = 1_000_000;
        
        // b.Power(e) long base
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < nTest; i++)
        {
            var b = (long)r.Next(2, 20);
            var exp = (long)r.Next(2, 14);
            b.Power(exp);
        }
        var time = sw.Elapsed.TotalSeconds;
        testOutput.WriteLine($"Running b.Power(e) with long base: {nTest / time:n0} / sec");

        // b.Power(e) ulong base
        sw = Stopwatch.StartNew();
        for (var i = 0; i < nTest; i++)
        {
            var b = (ulong)r.Next(2, 20);
            var exp = (ulong)r.Next(2, 14);
            b.Power(exp);
        }
        time = sw.Elapsed.TotalSeconds;
        testOutput.WriteLine($"Running b.Power(e) with ulong base: {nTest / time:n0} / sec");

        // b.BigPower(e) BigInteger-base
        sw.Restart();
        for (var i = 0; i < nTest; i++)
        {
            var b = new BigInteger(r.Next(2, 40));
            var exp = (int)r.Next(2, 20);
            var x = b.BigPower(exp);
        }
        time = sw.Elapsed.TotalSeconds;
        testOutput.WriteLine($"Running b.BigPower(e) with BigInteger base: {nTest / time:n0} / sec");

        // Math.Pow()
        sw.Restart();
        for (var i = 0; i < nTest; i++)
        {
            var b = (double)r.Next(2, 40);
            var exp = (double)r.Next(2, 20);
            var x = Math.Pow(b, exp);
        }
        time = sw.Elapsed.TotalSeconds;
        testOutput.WriteLine($"Running Math.Pow(): {nTest / time:n0} / sec");
    }

    [Fact(DisplayName = "Performance: ModPower()")]
    public void TestModPowerPerformance()
    {
        var r = new Random();
        int nTest = 1_000_000;

        var sw = Stopwatch.StartNew();
        for (var i = 0; i < nTest; i++)
        {
            var b = (ulong)r.Next(2, 20);
            var exp = (ulong)r.Next(2, 14);
            var m = (ulong)r.Next(2, 1_000_000_000);
            b.ModPower(exp, m);
        }
        var time = sw.Elapsed.TotalSeconds;
        testOutput.WriteLine($"Running b.ModPower(e,m): {nTest / time:n0} / sec");
    }

    [Fact(DisplayName = "Performance: Sqrt()")]
    public void TestSqrtPerformance()
    {
        var r = new Random();
        int nTest = 1_000_000;

        // BigInteger-Sqrt
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < nTest; i++)
        {
            var b = new BigInteger(r.Next(1_000_000_000, 2_000_000_000));
            b.Sqrt();
        }
        var time = sw.Elapsed.TotalSeconds;
        testOutput.WriteLine($"Running b.Sqrt(): {nTest / time:n0} / sec");

        // Math.Sqrt
        sw = Stopwatch.StartNew();
        for (var i = 0; i < nTest; i++)
        {
            var b = (double)r.Next(1_000_000_000, 2_000_000_000);
            Math.Sqrt(b);
        }
        time = sw.Elapsed.TotalSeconds;
        testOutput.WriteLine($"Running Math.Sqrt(): {nTest / time:n0} / sec");
    }

    #endregion
}
