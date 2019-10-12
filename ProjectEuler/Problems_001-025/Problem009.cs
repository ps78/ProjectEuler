using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=9
    /// 
    /// A Pythagorean triplet is a set of three natural numbers, a smaller b smaller c, for which,
    /// a^2 + b^2 = c^2    
    /// There exists exactly one Pythagorean triplet for which a + b + c = 1000. Find the product abc.
    /// </summary>
    public class Problem009 : EulerProblemBase
    {
        public Problem009() : base(9, "Special Pythagorean triplet", 1000, 31875000) { }

        public override bool Test() => Solve(12) == 60;

        public override long Solve(long n)
        {
            long result = 0;

            for (long a = 1; a <= n / 3; a++)
                for (long b = a + 1; b <= 2*n/3; b++)
                {
                    long c = n - a - b;
                    if ((a < b) && (b < c))
                        if (a * a + b * b == c * c)
                            result = a * b * c;
                }
            return result;
        }
    }
}
