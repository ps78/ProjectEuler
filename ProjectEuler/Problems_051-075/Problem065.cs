using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.IO;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=65
    /// The square root of 2 can be written as an infinite continued fraction.
    /// The infinite continued fraction can be written, √2 = [1;(2)], (2) indicates that 2 repeats ad infinitum.In a similar way, √23 = [4;(1,3,1,8)].
    /// 
    /// It turns out that the sequence of partial values of continued fractions for square roots provide the best rational approximations.Let us consider the convergents for √2.
    ///  
    /// Hence the sequence of the first ten convergents for √2 are:
    /// 
    /// 1, 3/2, 7/5, 17/12, 41/29, 99/70, 239/169, 577/408, 1393/985, 3363/2378, ...
    /// What is most surprising is that the important mathematical constant,
    /// e = [2; 1,2,1, 1,4,1, 1,6,1 , ... , 1,2k,1, ...].
    /// 
    /// The first ten terms in the sequence of convergents for e are:
    /// 
    /// 2, 3, 8/3, 11/4, 19/7, 87/32, 106/39, 193/71, 1264/465, 1457/536, ...
    /// The sum of digits in the numerator of the 10th convergent is 1+4+5+7=17.
    /// 
    /// Find the sum of digits in the numerator of the 100th convergent of the continued fraction for e.
    /// </summary>
    public class Problem065 : EulerProblemBase
    {
        public Problem065() : base(65, "Convergents of e", 100, 272) { }

        public override bool Test() => Solve(10) == 17;

        public override long Solve(long n)
        {
            var series = GetSeriesOfe((int)n).ToArray();
            BigInteger num, den;
            Simplify(series, out num, out den);

            return num.ToString().ToCharArray().Select<char, long>((c) => (long)((Byte)c - 48)).Sum();
        }

        private IEnumerable<int> GetSeriesOfe(int len)
        {
            yield return 2;
            for (int i = 1; i < len; i++)
                if ((i - 2) % 3 == 0)
                    yield return ((i - 2) / 3 + 1) * 2;
                else
                    yield return 1;
        }

        private void Simplify(int[] series, out BigInteger numerator, out BigInteger denominator)
        {
            var z = BigInteger.Zero;
            var n = BigInteger.One;

            for (int idx = series.Length - 1; idx >= 0; idx--)
            {
                if (idx == 0)
                    z = (ulong)series[0] * n + z;                    
                else
                {
                    BigInteger tmp = n;
                    n = new BigInteger(series[idx]) * n + z;
                    z = tmp;
                }
                /*
                var gcd = GCD.Compute(n, z);
                if (gcd != BigInteger.One)
                {
                    n /= gcd;
                    z /= gcd;
                }
                */
            }

            numerator = z;
            denominator = n;
        }
    }
}
