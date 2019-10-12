using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=6
    /// 
    /// The sum of the squares of the first ten natural numbers is,
    /// 12 + 22 + ... + 102 = 385
    /// The square of the sum of the first ten natural numbers is,
    /// (1 + 2 + ... + 10)2 = 552 = 3025
    /// Hence the difference between the sum of the squares of the first ten natural numbers and the square of the sum is 3025 − 385 = 2640.
    /// Find the difference between the sum of the squares of the first one hundred natural numbers and the square of the sum.
    /// </summary>
    public class Problem006 : EulerProblemBase
    {
        public Problem006() : base(6, "Sum square difference", 100, 25164150) { }

        public override bool Test() => Solve(10) == 2640;

        public override long Solve(long n)
        {
            long sum = 0;
            long sumSqr = 0;
            for (long i = 1; i <= n; i++)
            {
                sum += i;
                sumSqr += i * i;
            }
            return sum * sum - sumSqr;
        }
    }
}
