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
    /// https://projecteuler.net/problem=69
    /// Euler's Totient function, φ(n) [sometimes called the phi function], is used to determine the number of numbers less than n which are relatively prime to n. For example, as 1, 2, 4, 5, 7, and 8, are all less than nine and relatively prime to nine, φ(9)=6.
    /// n Relatively Prime φ(n)    n/φ(n)
    /// 2	1	1	2
    /// 3	1,2	2	1.5
    /// 4	1,3	2	2
    /// 5	1,2,3,4	4	1.25
    /// 6	1,5	2	3
    /// 7	1,2,3,4,5,6	6	1.1666...
    /// 8	1,3,5,7	4	2
    /// 9	1,2,4,5,7,8	6	1.5
    /// 10	1,3,7,9	4	2.5
    /// It can be seen that n = 6 produces a maximum n/φ(n) for n ≤ 10.
    /// 
    /// Find the value of n ≤ 1,000,000 for which n/φ(n) is a maximum.
    /// </summary>
    public class Problem069 : EulerProblemBase
    {
        public Problem069() : base(69, "Totient maximum", 1_000_000, 510510) { }

        public override bool Test() => Solve(10) == 6;

        public override long Solve(long n)
        {            
            var primes = new long[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };
            long product = 1;
            foreach (long p in primes)
            {
                if (product * p > n)
                    break;
                product *= p;
            }
            
            return product;
        }
    }
}
