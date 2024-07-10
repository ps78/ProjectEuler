using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace NumberTheory
{
    /// <summary>
    /// Euler's totient function / phi
    /// https://en.wikipedia.org/wiki/Euler%27s_totient_function
    /// 
    /// For N, phi(N) is the number of numbers smaller than N that are relatively prime to N
    /// 
    /// Note these rules:
    /// 
    /// phi(n) = n-1 if n is prime
    /// 
    /// phi(a* b) = phi(a) * phi(b)
    /// 
    /// phi(n) = (p1-1) * (p2-1) if n=p1* p2 and p1, p2 are prime factors of n
    /// 
    /// m^phi(n) = 1 mod n
    /// 
    /// </summary>
    public static class Totient
    {
        /// <summary>
        /// phi(n) can be computed as n * Product (1-1/p) where p are the distinct prime nubmers dividing n
        /// 
        /// sieve is an optional prime sieve. If provided, it's upper limit must be >= sqrt(n)
        /// </summary>
        public static ulong Compute(ulong n, SieveOfEratosthenes? sieve = null)
        {
            if (n == 1) return 1;

            if (sieve == null)
                sieve = new SieveOfEratosthenes(n);

            var primeTest = new SimplePrimeTest();

            if (primeTest.IsPrime(n))
                return n - 1;
            else
                return ComputeForNonPrimes(n, sieve);                
        }

        /// <summary>
        /// Fast compute if n is not a prime. 
        /// 
        /// sieve is an optional prime sieve. If provided, it's upper limit must be >= sqrt(n)
        /// </summary>
        public static ulong ComputeForNonPrimes(ulong n, SieveOfEratosthenes? sieve = null)
        {
            if (n == 1) return 1;

            if (sieve == null)
                sieve = new SieveOfEratosthenes(n);
            
            double result = 1;
            foreach (var f in sieve.GetPrimeFactors(n))
            {
                result *= (1.0 - 1.0 / f.Item1);
            }
            return (ulong)Math.Round(n * result);
        }
    }
}
