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
    /// https://projecteuler.net/problem=49
    /// The arithmetic sequence, 1487, 4817, 8147, in which each of the terms increases by 3330, is unusual in two ways: 
    /// (i) each of the three terms are prime, and, 
    /// (ii) each of the 4-digit numbers are permutations of one another.
    /// There are no arithmetic sequences made up of three 1-, 2-, or 3-digit primes, exhibiting this property, 
    /// but there is one other 4-digit increasing sequence.
    /// What 12-digit number do you form by concatenating the three terms in this sequence?
    /// </summary>
    public class Problem049 : EulerProblemBase
    {
        public Problem049() : base(49, "Prime permutations", 0, 296962999629) { }

        public override long Solve(long n)
        {
            var sieve = new SieveOfEratosthenes(10_000);
            var primes = sieve.GetPrimes(1000, 9999);
            int nPrimes = primes.Count;

            ulong solution = 0;
            for (int i = 0; i < nPrimes; i++)
            {
                ulong p1 = primes[i];

                if (p1 == 1487) // this solution is already known..
                    continue;

                for (int j = i + 1; j < nPrimes; j++)
                {
                    ulong p2 = primes[j];
                    ulong p3 = 2 * p2 - p1;
                    if ((p3 < 10_000) && sieve.IsPrime(p3) && IsSolution(p1, p2, p3))
                    {
                        solution = p1 * 100_000_000 + p2 * 10_000 + p3;               
                        break;
                    }
                }
                if (solution != 0)
                    break;
            }

            return (long)solution;
        }

        private bool IsSolution(ulong p1, ulong p2, ulong p3)
        {
            var ps1 = p1.ToString().ToCharArray().OrderBy(c => c);
            var ps2 = p2.ToString().ToCharArray().OrderBy(c => c);
            var ps3 = p3.ToString().ToCharArray().OrderBy(c => c);

            return ps1.SequenceEqual(ps2) && ps1.SequenceEqual(ps3);
        }

    }
}
