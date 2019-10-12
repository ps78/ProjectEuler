using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=35
    ///
    /// The number, 197, is called a circular prime because all rotations of the digits: 197, 971, and 719, are themselves prime.
    /// There are thirteen such primes below 100: 2, 3, 5, 7, 11, 13, 17, 31, 37, 71, 73, 79, and 97.
    /// How many circular primes are there below one million?
    /// </summary>
    public class Problem035 : EulerProblemBase
    {
        public Problem035() : base(35, "Circular primes", 1_000_000, 55) { }

        public override bool Test() => Solve(100) == 13;

        private SieveOfEratosthenes Sieve;

        public override long Solve(long n)
        {
            Sieve = new SieveOfEratosthenes((ulong)n);
            var result = new List<ulong>();
            foreach (var p in Sieve.GetPrimes())
                if (IsCircularPrime(p))
                    result.Add(p);

            return result.Count;
        }

        public bool IsCircularPrime(ulong n)
        {
            string s = n.ToString();
            int len = s.Length;            
            for (int i = 1; i < len; i++)
            {
                s = s.Substring(1) + s[0];                
                if (!Sieve.IsPrime(ulong.Parse(s)))
                    return false;
            }
            return true;
        }
    }
}
