using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=7
    /// 
    /// By listing the first six prime numbers: 2, 3, 5, 7, 11, and 13, we can see that the 6th prime is 13.
    /// What is the 10001st prime number?
    /// </summary>
    public class Problem007 : EulerProblemBase
    {
        public Problem007() : base(7, "10001st prime", 10001, 104743) { }

        public override bool Test() => Solve(6) == 13;

        public override long Solve(long n)
        {
            var estimatedLimit = (ulong)(Math.Log(ProblemSize) * n * 2);
            var sieve = new SieveOfEratosthenes(estimatedLimit);
            var primes = sieve.GetPrimes();

            if (primes.Count < (int)n)
                throw new InvalidOperationException("The sieve of Eratostenes didn't return enough primes. Incrase the estimated limit");
            else
                return (long)primes[(int)(n - 1)];
        }
    }
}
