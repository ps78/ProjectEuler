using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=48
    /// The series, 11 + 22 + 33 + ... + 1010 = 10405071317.
    /// Find the last ten digits of the series, 11 + 22 + 33 + ... + 10001000.
    /// </summary>
    public class Problem048 : EulerProblemBase
    {
        public Problem048() : base(48, "Self powers", 1000, 9110846700) { }

        public override bool Test() => Solve(10) == (10405071317 % 10_000_000_000);

        public override long Solve(long n)
        {
            long result = 0;
            for (long i = 1; i <= n; i++)
                result += SelfPower(i);

            return result % 10_000_000_000;
        }

        /// <summary>
        /// calculates last 10 digits of i^i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private long SelfPower(long n)
        {
            long result = 1;
            for (int i = 0; i < (int)n; i++)
                result = (result * n) % 10_000_000_000;
            return result;
        }

    }
}
