using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=46
    /// 
    /// It was proposed by Christian Goldbach that every odd composite number can be written as the sum of a prime and twice a square.
    /// 9 = 7 + 2×1^2
    /// 15 = 7 + 2×2^2
    /// 21 = 3 + 2×3^2
    /// 25 = 7 + 2×3^2
    /// 27 = 19 + 2×2^2
    /// 33 = 31 + 2×1^2
    /// It turns out that the conjecture was false.
    /// What is the smallest odd composite that cannot be written as the sum of a prime and twice a square?
    /// </summary>
    public class Problem046 : EulerProblemBase
    {
        public Problem046() : base(46, "Goldbach's other conjecture", 0, 5777) { }

        private SieveOfEratosthenes sieve;

        public override long Solve(long n)
        {
            sieve = new SieveOfEratosthenes(10000);

            ulong i = 9;
            while (true)
            {
                if (!sieve.IsPrime(i))
                {
                    if (!TestNumber(i))
                        return (long)i;
                }
                i += 2;
            }                        
        }

        /// <summary>
        /// tests if n can be written as p + 2*q^2
        /// </summary>
        private bool TestNumber(ulong n)
        {
            ulong maxRoot = (ulong)Math.Sqrt((n - 3) / 2);
            for (ulong root = 1; root <= maxRoot; root++)
            {
                // compute p from n = p+2*root^2
                ulong p = n - 2 * root * root;
                if (sieve.IsPrime(p))
                    return true;
            }
            return false;
        }
    }
}
