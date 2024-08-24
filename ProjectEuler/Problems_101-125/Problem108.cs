using NumberTheory;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=108
/// 
/// In the following equation x, y, and n are positive integers.
/// 
///     1/x + 1/y = 1/n
///     
/// For n = 4 there are exactly three distinct solutions:
/// 
///     1/5 + 1/20 = 1/4
///     1/6 + 1/12 = 1/4
///     1/8 + 1/8  = 1/4
///     
/// What is the least value of n for which the number of distinct solutions exceeds one-thousand?
/// 
/// NOTE: This problem is an easier version of Problem 110; it is strongly advised that you solve this one first.
/// </summary>
public class Problem108 : EulerProblemBase
{
    public Problem108() : base(108, "Diophantine Reciprocals I", 1000, 180180) { }

    /// <summary>
    /// 1/x + 1/y = 1/n
    /// (x+y) / xy = 1/n
    /// n*(x+y) = x*y
    /// n*x + n*y = x*y
    /// n*x = y(x-n)
    /// y = nx / (x-n)
    /// 
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public override long Solve(long n)
    {     
        foreach (var (m, fcount) in NumSequence(10000000))
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
    private List<(long,long)> NumSequence(long limit)
    {
        int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };

        var numbers = new List<(long,long)>();

        for (int i = 1; i < primes.Length; i++)
        {
            var factors = primes[..i];
            var exponents = Enumerable.Repeat(1, factors.Length).ToArray();

            long p = factors.Product();
            if (p < limit)
            {
                numbers.Add((p, 2L.Power(factors.Length)));
                NumSequenceRecursive(limit, p, ref numbers, ref factors, ref exponents, 0);
            }
        }

        return numbers.OrderBy(x => x.Item1).ToList();
    }

    private void NumSequenceRecursive(long limit, long baseProduct, ref List<(long,long)> numbers, ref int[] factors, ref int[] exponents, int index)
    {
        int maxExp = (index == 0) ? int.MaxValue : exponents[index - 1];

        for (int exp = 2; exp <= maxExp; exp++)
        {
            exponents[index] = exp;
            long num = baseProduct * ((long)factors[index]).Power(exp-1);
            if (num >= limit)
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
