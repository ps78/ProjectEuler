using NumberTheory;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=110
/// 
/// In the following equation x, y, and n are positive integers.
/// 
///     1/x + 1/y = 1/n
///     
/// It can be verified that when n=1260 there are 113 distinct solutions and this is the least value of n
/// for which the total number of distinct solutions exceeds one hundred.
///
/// What is the least value of n for which the number of distinct solutions exceeds four million?
/// NOTE: This problem is a much more difficult version of Problem 108 and as it is well beyond the 
/// limitations of a brute force approach it requires a clever implementation.
/// </summary>
public class Problem110 : EulerProblemBase
{
    public Problem110() : base(110, "Diophantine Reciprocals II", 4_000_000, 9350130049860600) { }

    public override long Solve(long n)
    {
        var nums = NumSequence(10_000_000_000_000_000, 50);

        foreach (var (m, fcount) in nums)
            if ((fcount + 1) / 2 > n)
                return m;
        return 0;
    }

    /// <summary>
    /// Generates all numbers smaller than limit whose prime factorization
    /// contains all consequtive primes up to the largest one
    /// and the exponents are non-increasing and the exponent of
    /// the largest factor is one
    /// Numbers are returned ordered
    /// </summary>
    /// <returns>list of (n, number of factors of n)</returns>
    private List<(long,long)> NumSequence(long limit, ulong primeLimit)
    {
        long[] primes = new SieveOfEratosthenes(primeLimit).GetPrimes().Select(p => (long)p).ToArray();

        var numbers = new List<(long,long)>();

        for (int i = 1; i < primes.Length; i++)
        {
            var factors = primes[..i];
            var exponents = Enumerable.Repeat(1L, factors.Length).ToArray();

            long p = factors.Product();
            if (p < limit)
            {
                numbers.Add((p, 2L.Power(factors.Length)));
                NumSequenceRecursive(limit, p, ref numbers, ref factors, ref exponents, 0);
            }
        }

        return numbers.OrderBy(x => x.Item1).ToList();
    }

    private void NumSequenceRecursive(long limit, long baseProduct, ref List<(long,long)> numbers, ref long[] factors, ref long[] exponents, int index)
    {
        long maxExp = (index == 0) ? int.MaxValue : exponents[index - 1];

        for (long exp = 2; exp <= maxExp; exp++)
        {
            exponents[index] = exp;
            long num = baseProduct * (factors[index]).Power(exp-1);
            if (num >= limit || num < 0)
            {
                break;
            }
            else
            {
                long numFactors = exponents.Select(e => 2*e + 1).Product();
                numbers.Add((num, numFactors));
                if (index < factors.Length-1)
                    NumSequenceRecursive(limit, num, ref numbers, ref factors, ref exponents, index + 1);
            }
        }
        exponents[index] = 1;
    }
}
