using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=10
    /// 
    /// The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.
    /// Find the sum of all the primes below two million.
    /// </summary>
    public class Problem010 : EulerProblemBase
    {
        public Problem010() : base(10, "Summation of primes", 2_000_000, 142913828922) { }

        public override bool Test() => Solve(10) == 17;

        public override long Solve(long n)
        {
            var sieve = new SieveOfEratosthenes((ulong)Math.Max(n, 30));

            var primes = sieve.GetPrimes(2, (ulong)n - 1);

            return primes.Aggregate((long)0, (total, next) => total += (long)next);
        }
    }
}
