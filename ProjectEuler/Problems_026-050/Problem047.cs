using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    using Factors = IEnumerable<Tuple<ulong, ulong>>;

    /// <summary>
    /// https://projecteuler.net/problem=47
    /// The first two consecutive numbers to have two distinct prime factors are:
    /// 
    /// 14 = 2 × 7
    /// 15 = 3 × 5
    /// 
    /// The first three consecutive numbers to have three distinct prime factors are:
    /// 
    /// 644 = 2² × 7 × 23
    /// 645 = 3 × 5 × 43
    /// 646 = 2 × 17 × 19.
    /// 
    /// Find the first four consecutive integers to have four distinct prime factors.What is the first of these numbers?
    /// </summary>
    public class Problem047 : EulerProblemBase
    {
        public Problem047() : base(47, "Distinct primes factors", 4, 134043) { }
        
        private SieveOfEratosthenes sieve;

        public override bool Test() => Solve(2) == 14 && Solve(3) == 644;

        public override long Solve(long n)
        {
            Prepare((ulong)n);
            int p = (int)n;

            // initialize working set
            var isNDistinct = new bool[n];
            for (ulong i = 0; i < (ulong)n; i++)
                isNDistinct[i] = sieve.GetPrimeFactors(2 + i).Select(f => f.Item1).Distinct().Count() >= p;

            int k = 0;
            ulong solution = 0;
            for (ulong i = 2 + (ulong)n; true; i++)
            {                
                if (isNDistinct.All(b => b))
                {
                    solution = i - (ulong)n;
                    break;
                }

                isNDistinct[k] = (sieve.GetPrimeFactors(i).Select(f => f.Item1).Distinct().Count() == p);
                k = (k + 1) % p;
            }

            return (long)solution;
        }

        private void Prepare(ulong n)
        {
            switch(n)
            {
                case 2:
                case 3: sieve = new SieveOfEratosthenes(1000); break;
                case 4: sieve = new SieveOfEratosthenes(140_000); break;
                case 5: sieve = new SieveOfEratosthenes(10_000_000); break;
            }
        }

    }
}
