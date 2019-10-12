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
    /// https://projecteuler.net/problem=97
    /// The first known prime found to exceed one million digits was discovered in 1999, 
    /// and is a Mersenne prime of the form 26972593−1; it contains exactly 2,098,960 digits. 
    /// Subsequently other Mersenne primes, of the form 2p−1, have been found which contain more digits.
    /// 
    /// However, in 2004 there was found a massive non-Mersenne prime which contains 2,357,207 digits: 28433×2^7830457+1.
    /// 
    /// Find the last ten digits of this prime number.
    /// </summary>
    public class Problem097 : EulerProblemBase
    {
        public Problem097() : base(97, "Large non-Mersenne prime", 0, 8739992577) { }

        public override long Solve(long n)
        {
            long k = 1;

            for (long i = 1; i <= 7830457; i++)            
                k = (k * 2) % 10_000_000_000;

            k = (k * 28433 + 1) % 10_000_000_000;

            return k;
        }
    }
}
