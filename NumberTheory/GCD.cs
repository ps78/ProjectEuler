using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NumberTheory
{
    /// <summary>
    /// Greates commond divisor
    /// </summary>
    public static class GCD
    {
        /// <summary>
        /// uses Euclids method, see https://en.wikipedia.org/wiki/Greatest_common_divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ulong Compute(ulong a, ulong b)
        {
            if (a == 0 || b == 0)
                return Math.Max(a, b);

            ulong n1 = a;
            ulong n2 = b;
            while (true)
            {
                if (n1 == n2)
                    return n1;

                if (n1 > n2)
                {
                    n1 = n1 - n2;                    
                }
                else if (n1 < n2)
                {
                    n2 = n2 - n1;
                }
            }            
        }

        public static BigInteger Compute(BigInteger a, BigInteger b)
        {
            BigInteger n1 = a;
            BigInteger n2 = b;
            while (true)
            {
                if (n1 == n2)
                    return n1;

                if (n1 > n2)
                {
                    n1 = n1 - n2;
                }
                else if (n1 < n2)
                {
                    n2 = n2 - n1;
                }
            }
        }
    }
}
