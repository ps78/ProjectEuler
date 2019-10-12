using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.IO;
using System.Numerics;
using System.Diagnostics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=60
    /// The primes 3, 7, 109, and 673, are quite remarkable. By taking any two primes and concatenating them in any order 
    /// the result will always be prime. For example, taking 7 and 109, both 7109 and 1097 are prime. 
    /// 
    /// The sum of these four primes, 792, represents the lowest sum for a set of four primes with this property.
    /// 
    /// Find the lowest sum for a set of five primes for which any two primes concatenate to produce another prime.
    /// </summary>
    public class Problem060 : EulerProblemBase
    {
        public Problem060() : base(60, "Prime pair sets", 0, 26033) { }

        private SieveOfEratosthenes sieve;
        public const ulong SieveLimit = 10_000_000; // this can be set lower or higher, 10 mio seems to be a good value.
        private ulong[] prm;        // list of consequtive primes from 3 to maxPrime, without 5
        private ulong[] prmSqr;      // squares of the primes, use for fast prime test

        public override long Solve(long n)
        {
            sieve = new SieveOfEratosthenes(SieveLimit);

            var primeList = sieve.GetPrimes(3, 9999);
            primeList.Remove(5); // we don't want 2 and 5, these can never be part of the solution
            prm = primeList.ToArray();
            prmSqr = primeList.Select(i => i * i).ToArray(); 
            int M = prm.Length;

            // create an array of multipliers for each prime, so they can later be concatenated.
            // 1-9 => 10, 10-99 => 100, etc.
            var multiplier = prm.Select(p => (ulong)Math.Pow(10, Math.Floor(Math.Log10(p*10)))).ToArray();

            // 2D array where we store true for each pair of primes that are prime when concatenated
            var check = new bool[M, M];

            for (int i1 = 0; i1 < M; i1++)
                for (int i2 = i1 + 1; i2 < M; i2++)
                    if (IsPrime(prm[i1] * multiplier[i2] + prm[i2]) && IsPrime(prm[i2] * multiplier[i1] + prm[i1]))
                        check[i1, i2] = true;

            // now test all pairs in each possible 5-tuple of primes
            for (int i1 = 0; i1 < M; i1++)
                for (int i2 = i1 + 1; i2 < M; i2++)
                    if (check[i1, i2])
                        for (int i3 = i2 + 1; i3 < M; i3++)
                            if (check[i1, i3] && check[i2, i3])
                                for (int i4 = i3 + 1; i4 < M; i4++)
                                    if (check[i1, i4] && check[i2, i4] && check[i3, i4])
                                        for (int i5 = i4 + 1; i5 < M; i5++)
                                            if (check[i1, i5] && check[i2, i5] && check[i3, i5] && check[i4, i5])
                                                return (long)(prm[i1] + prm[i2] + prm[i3] + prm[i4] + prm[i5]);

            return 0;
        }

        private bool IsPrime(ulong n)
        {            
            if (n < SieveLimit)
                return sieve.IsPrime(n);

            // test n against divisibility of primes smaller than sqrt(n), without actually calculating sqrt(n)
            // Note that Sqrt(n) must be smaller than the prime-sieve-limit, which is guaranteed
            for (int i = 0; i < prm.Length && prmSqr[i] <= n; i++)
                if (n % prm[i] == 0)
                    return false;

            return true;
        }
    }
}
