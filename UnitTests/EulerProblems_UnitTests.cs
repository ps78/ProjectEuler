using System;
using System.Diagnostics;
using ProjectEuler;
using System.Linq;
using System.Collections;
using System.Numerics;
using NumberTheory;
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

namespace UnitTests;

public class EulerProblems_UnitTests
{
    #region Setup / Helper

    private readonly ITestOutputHelper testOutput;

    public EulerProblems_UnitTests(ITestOutputHelper testOutputHelper)
    {
        testOutput = testOutputHelper;
    }
    #endregion

    [Fact(DisplayName = "Run Test() methods of all problems")]
    public void TestProblemsFast()
    {
        var pm = new ProblemManager();

        // only test those problems that have a solution
        foreach (var p in pm.Problems.Where(x => x.IsSolved))
        {
            bool testResult = p.Test();
            
            if (!testResult)
                testOutput.WriteLine($"Problem {p.ProblemNumber} did not pass Test()");

            testResult.Should().BeTrue();
        }
    }

    [Fact(DisplayName = "Run Solve() methods of all problems")]
    public void TestProblemsFull()
    {
        var pm = new ProblemManager();
        int wrongCount = 0, okCount = 0, skipCount = 0;

        // only run for problems that have a solution
        foreach (var p in pm.Problems)
        {
            if (p.IsSolved)
            {
                var solution = p.Solve(p.ProblemSize);
                if (solution != p.Solution)
                {
                    testOutput.WriteLine($"{p.ProblemNumber,3:D3}: WRONG / ACTUAL = {solution,26:N0} / EXPECTED = {p.Solution}");
                    wrongCount++;
                }
                else
                    okCount++;
            }
            else
            {
                testOutput.WriteLine($"{p.ProblemNumber,3:D3}: SKIPPED (solution not known)");
                skipCount++;
            }
        }
        testOutput.WriteLine($"{okCount} problems solved correctly");
        testOutput.WriteLine($"{skipCount} problems skipped");
        testOutput.WriteLine($"{wrongCount} problems solved incorrectly");
        wrongCount.Should().Be(0);
    }

    /*
     * TODO: fix this, the tests using files don't work because the path is relative to the original project folder
    [TestMethod]
    public void TestProblemsFull()
    {
        var pm = new ProblemManager();

        // only test those problems that have a solution
        foreach (var p in pm.Problems.Where(x => x.IsSolved))
            Assert.IsTrue(p.Solve(p.ProblemSize) == p.Solution);
    }
    */
}
