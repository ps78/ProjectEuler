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
    /// https://projecteuler.net/problem=56
    /// A googol (10100) is a massive number: one followed by one-hundred zeros; 100100 is almost unimaginably large:
    /// one followed by two-hundred zeros. Despite their size, the sum of the digits in each number is only 1.
    /// Considering natural numbers of the form, ab, where a, b< 100, what is the maximum digital sum?
    /// </summary>
    public class Problem056 : EulerProblemBase
    {
        public Problem056() : base(56, "Powerful digit sum", 0, 972) { }

        public override long Solve(long n)
        {
            var max = new BigInteger(0);
            int maxa = 0, maxb = 0;
            for (int a = 50; a < 100; a++)
            {
                BigInteger m = new BigInteger(1);
                for (int b = 1; b < 100; b++)
                {
                    m *= a;
                    var sum = DigitalSum(m);
                    if (sum > max)
                    {
                        max = sum;
                        maxa = a;
                        maxb = b;
                    }
                }
            }  
            //Console.WriteLine("{0}^{1} -> {2}", maxa, maxb, max);
            //Console.WriteLine("99^99 = {0}", (99).BigPower(99));
            //Console.WriteLine("99^94 = {0}", (99).BigPower(94));

            return (long)max;
        }
        
        public int DigitalSum(BigInteger n)
        {
            int sum = 0;
            foreach (char c in n.ToString().ToCharArray())
                sum += ((byte)c) - 48;
            return sum;
        }

    }
}
