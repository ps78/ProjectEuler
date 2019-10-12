using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=71
    /// Consider the fraction, n/d, where n and d are positive integers.If n less than d and HCF(n, d)=1, 
    /// it is called a reduced proper fraction.
    /// If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get:
    /// 
    /// 1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8
    /// 
    /// It can be seen that 2/5 is the fraction immediately to the left of 3/7.
    /// By listing the set of reduced proper fractions for d ≤ 1,000,000 in ascending order of size, 
    /// find the numerator of the fraction immediately to the left of 3/7.
    /// </summary>
    public class Problem071 : EulerProblemBase
    {
        public Problem071() : base(71, "Ordered fractions", 1_000_000, 428570) { }

        // TODO: test not working
        //public override bool Test() => Solve(8) == 2;

        public override long Solve(long n)
        {
            double minDiff = double.MaxValue;
            ulong bestN = 1, bestD = 1;

            for (ulong d = 8; d <= (ulong)n; d++)
            {
                ulong m = 3 * d / 7;
                double diff = 3.0 / 7 - (double)m / d;
                if ((diff < minDiff) && (diff > 0))
                {
                    bestN = m;
                    bestD = d;
                    minDiff = diff;
                }
            }

            ulong gcd = GCD.Compute(bestN, bestD);
            bestN /= gcd;
            bestD /= gcd;

            return (long)bestN;
        }
    }
}
