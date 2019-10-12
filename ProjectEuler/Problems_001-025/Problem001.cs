using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=1
    /// 
    /// If we list all the natural numbers below 10 that are multiples of 3 or 5, we get 3, 5, 6 and 9. The sum of these multiples is 23.
    /// Find the sum of all the multiples of 3 or 5 below 1000.
    /// 
    /// </summary>
    public class Problem001 : EulerProblemBase
    {
        public Problem001() : base(1, "Multiples of 3 and 5", 1000, 233168) { }

        public override bool Test() => Solve(10) == 23;

        public override long Solve(long n)
        {
            long sum = 0;
            for (long i = 3; i < n; i++)
                if (((i % 3) == 0) ||((i % 5) == 0))
                    sum += i;
            return sum;            
        }
    }
}
