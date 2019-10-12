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
    /// https://projecteuler.net/problem=70
    /// Euler's Totient function, φ(n) [sometimes called the phi function], is used to determine the number of positive numbers 
    /// less than or equal to n which are relatively prime to n. For example, as 1, 2, 4, 5, 7, and 8, are all 
    /// less than nine and relatively prime to nine, φ(9)=6.
    /// 
    /// The number 1 is considered to be relatively prime to every positive number, so φ(1)=1.
    /// 
    /// Interestingly, φ(87109)=79180, and it can be seen that 87109 is a permutation of 79180.
    /// 
    /// Find the value of n between 1 and 10^7 (exclusive), for which φ(n) is a permutation of n and the ratio n/φ(n) produces a minimum.
    /// </summary>
    public class Problem070 : EulerProblemBase
    {
        public Problem070() : base(70, "Totient permutation", 10_000_000, 8319823) { }

        public override long Solve(long n)
        {
            /* Note: 
             * phi(n) = n-1 if n is prime
             * phi(a*b) = phi(a) * phi(b)
             * phi(n) = (p1-1) * (p2-1) if n=p1*p2 and p1, p2 are prime factors of n
             * m^phi(n) = 1 mod n
             * 
             * Conclusions:
             * 1) since phi(n) = n-1 if n is prime -> the result cannot be a prime, as p and p-1 cannot be a permutation
             * 2) the relative difference of n and phi(n) should be as small as possible -> phi(n) as large as possible
             * 3) since phi(n) = product of (pi-1) for pi = primefactors of n, the smalles primefactor of n should be as large as possible
             * 4) from 3) follows that n should have 2 prime factors, of about equal size
             * 5) strategy: multiply all pairs of primes that are about sqrt(10'000'000) and test these
             * 
             * */
            ulong sqrt = (ulong)Math.Sqrt(ProblemSize);
            ulong ulim = 2 * sqrt;
            ulong llim = (ulong)n / ulim;
            double min = double.MaxValue;
            var sieve = new SieveOfEratosthenes(ulim);
            var primes = sieve.GetPrimes(llim, ulim);
            ulong bestN = 0;
            for (int i = 0; i < primes.Count; i++)
                for (int j = i; j < primes.Count; j++)
                {
                    ulong m = primes[i] * primes[j];
                    if (m < (ulong)n)
                    {
                        ulong phi = (primes[i] - 1) * (primes[j] - 1);
                        if (phi.ToString().IsPermuationOf(m.ToString()))
                        {
                            double ratio = (double)m / phi;
                            if (ratio < min)
                            {
                                min = ratio;
                                bestN = m;
                            }
                        }
                    }
                }
            
            return (long)bestN;
        }
    }
}
