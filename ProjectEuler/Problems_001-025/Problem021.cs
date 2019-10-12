using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=21
    /// 
    /// Let d(n) be defined as the sum of proper divisors of n (numbers less than n which divide evenly into n).
    /// If d(a) = b and d(b) = a, where a ≠ b, then a and b are an amicable pair and each of a and b are called amicable numbers.
    /// For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110; therefore d(220) = 284. 
    /// The proper divisors of 284 are 1, 2, 4, 71 and 142; so d(284) = 220.
    /// Evaluate the sum of all the amicable numbers under 10000.
    /// </summary>
    public class Problem021 : EulerProblemBase
    {
        public Problem021() : base(21, "Amicable numbers", 10000, 31626) { }

        public override bool Test() => Solve(1000) == 220 + 284;

        public override long Solve(long n)
        {
            var sieve = new SieveOfEratosthenes((ulong)n);
            var primes = sieve.GetPrimes().ToArray();

            var sums = new long[n];
            for (long i = 1; i < n; i++)
                sums[i] = (long)sieve.GetFactors((ulong)i, primes).Reverse().Skip(1).Sum();

            long sum = 0;
            for (long i = 1; i < n; i++)
            {
                var j = sums[i];
                if ((i != j) && (j < n) && (sums[j] == i))
                    sum += i;
            }
                
            return sum;
        }
    }
}
