using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=5
    ///
    /// 2520 is the smallest number that can be divided by each of the numbers from 1 to 10 without any remainder.
    /// What is the smallest positive number that is evenly divisible by all of the numbers from 1 to 20?
    /// 
    /// </summary>
    public class Problem005 : EulerProblemBase
    {
        public Problem005() : base(5, "Smallest multiple", 20, 232792560) { }

        public override bool Test() => Solve(10) == 2520;

        /// <summary>
        /// strategy: create all prime factors of the number from 1 to ProblemSize.
        /// The solution is the multiplication of these factors
        /// </summary>
        /// <returns></returns>
        public override long Solve(long n)
        {
            var primeFactors = new Dictionary<ulong, ulong>(); // index = factor, value = exponent
            ulong sieveSize = Math.Max(30, (ulong)Math.Sqrt(n));
            var sieve = new SieveOfEratosthenes(sieveSize);
            var primes = sieve.GetPrimes();

            for (ulong i = 2; i <= (ulong)n; i++)
            {
                if (sieve.IsPrime(i))
                {
                    primeFactors.Add(i, 1);
                }
                else
                {
                    ulong limit = (ulong)(Math.Sqrt(i));
                    int pi = 0;
                    while (primes[pi] <= limit)
                    {
                        if (i % primes[pi] == 0)
                        {
                            ulong exp = 1;
                            ulong m = i / primes[pi];
                            do
                            {
                                if (m % primes[pi] == 0)
                                {
                                    m = m / primes[pi];
                                    exp++;
                                }
                                else
                                    break;
                            } while (true);

                            if (!primeFactors.ContainsKey(primes[pi]))
                                primeFactors.Add(primes[pi], exp);
                            else if (primeFactors[primes[pi]] < exp)
                                primeFactors[primes[pi]] = exp;
                        }
                        pi++;
                    }
                }               
            }

            ulong product = 1;
            foreach (var factor in primeFactors)
                product *= IntegerExp(factor.Key, factor.Value);

            return (long)product;
        }

        public static ulong IntegerExp(ulong baseValue, ulong exponent)
        {
            ulong result = baseValue;
            for (ulong i = 2; i <= exponent; i++)
                result *= baseValue;
            return result;  
        }
    }
}
