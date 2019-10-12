using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=20
    ///    n! means n × (n − 1) × ... × 3 × 2 × 1
    ///
    ///    For example, 10! = 10 × 9 × ... × 3 × 2 × 1 = 3628800,
    ///    and the sum of the digits in the number 10! is 3 + 6 + 2 + 8 + 8 + 0 + 0 = 27.
    ///
    ///    Find the sum of the digits in the number 100!
    /// </summary>
    public class Problem020 : EulerProblemBase
    {
        public Problem020() : base(20, "Factorial digit sum", 100, 648) { }

        public override bool Test() => Solve(10) == 27;

        public override long Solve(long n)
        {
            var m = new BigInteger(2);
            for (long i = 3; i < n; i++)
                m = m * i;

            return m.ToString().ToCharArray().Sum((c) => int.Parse(c.ToString()));
        }
    }
}
