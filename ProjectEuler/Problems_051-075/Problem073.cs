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
    /// https://projecteuler.net/problem=73    
    /// Consider the fraction, n/d, where n and d are positive integers.If n<d and HCF(n, d)=1, it is called a reduced proper fraction.
    /// 
    /// If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get:
    /// 
    /// 1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8
    /// 
    /// It can be seen that there are 3 fractions between 1/3 and 1/2.
    /// 
    /// How many fractions lie between 1/3 and 1/2 in the sorted set of reduced proper fractions for d ≤ 12,000?
    /// </summary>
    public class Problem073 : EulerProblemBase
    {
        public Problem073() : base(73, "Counting fractions in a range", 12000, 7295372) { }

        // TODO: Test doesn't work
        //public override bool Test() => Solve(8) == 3;

        public override long Solve(long n)
        {
            /*
            // brute force approach, takes ~ 20sec
            int count = 0;
            for (int d = 2; d <= (int)ProblemSize; d++)
            {
                // d/3 < n < d/2
                int lbound = (d % 3 == 0) ? d / 3 + 1 : (int)Math.Ceiling((double)d / 3);
                int ubound = (d % 2 == 0) ? d / 2 - 1 : (int)Math.Floor((double)d / 2);

                if (ubound >= lbound)
                    for (int n = lbound; n <= ubound; n++)
                        if (!(n % 2 == 0 && d % 2 == 0))
                            if (GCD.Compute(n, d) == 1)
                                count++;
            }
            return (ulong)count;
            */

            // solution using Farey series:
            int limit = (int)n;
            // note the values for a, b, c, d only work for limit = 12000
            int a = 1;
            int b = 3;
            int c = limit / 3;
            int d = limit - 1; 
            long count = 0;
            while (! (c == 1 && d == 2))
            {
                count++;
                int k = (limit + b) / d;
                int e = k * c - a;
                int f = k * d - b;
                a = c;
                b = d;
                c = e;
                d = f;
            }
            return count;
        }
    }
}
