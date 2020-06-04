using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=200
    /// 
    /// We shall define a sqube to be a number of the form, p2q3, where p and q are distinct primes.
    /// For example, 200 = 5^2*2^3 or 120072949 = 23^2*61^3.
    /// 
    /// The first five squbes are 72, 108, 200, 392, and 500.
    /// 
    /// Interestingly, 200 is also the first number for which you cannot change any single digit to make a prime; 
    /// we shall call such numbers, prime-proof.The next prime-proof sqube which contains the contiguous sub-string "200" is 1992008.
    /// 
    /// Find the 200th prime-proof sqube containing the contiguous sub-string "200".
    /// </summary>
    public class Problem200 : EulerProblemBase
    {
        public Problem200() : base(200, "Find the 200th prime-proof sqube containing '200'", 200, 229161792008) { }

        public override bool Test() => Solve(2) == 1992008;

        private SieveOfEratosthenes sieve;
        private MillerRabinTest mr;
        private Dictionary<ulong, (ulong, ulong)> squbes = new Dictionary<ulong, (ulong, ulong)>();
        private ulong max_q; // largest q used so far to generate squbes

        public override long Solve(long n)
        {
            // dictionary of squbes=p^2 * q^3, where values are (p, q)
            squbes.Clear();
            squbes.Add(72, (3, 2));
            squbes.Add(108, (2, 3));
            max_q = 3;

            sieve = new SieveOfEratosthenes(200000);
            mr = new MillerRabinTest();            

            long count = 0;
            while (true)
            {
                var sqube = NextSqube();
                if (sqube.sqube.ToString().Contains("200"))
                    if (IsPrimeProof(sqube.sqube))
                    {
                        count++;
                        //Console.WriteLine($"{sqube.sqube} : p = {sqube.p} q = {sqube.q}");                        
                        if (count >= n)
                            return (long)sqube.sqube;
                    }
            }
        }

        /// <summary>
        /// Returns the next prime > n and stores all primes
       /// generated on the way in the internal _primes list
        /// </summary>
        private ulong next_prime(ulong n)
        {
            // create larger sieve if necessary
            if (n + 100 > sieve.Limit)
                sieve = new SieveOfEratosthenes(2 * sieve.Limit);

            var p = (n % 2) == 0 ? n + 1 : n + 2;
            while (!sieve.IsPrime(p))
                p += 2;

            return p;
        }

        /// <summary>
        /// Calculates the next Sqube, stores it in self._squbes and also returns it, together with
        /// the generating p and q, hence the return value is the tuple:
        /// (sqube, p, q) where sqube = p**2 * q**3
        /// </summary>
        /// <returns></returns>
        private (ulong sqube, ulong p, ulong q) NextSqube()
        {
            // the smallest sqube in the list is the next one. Remove it
            var next_sqube = squbes.Keys.Min();
            var (p, q) = squbes[next_sqube];
            squbes.Remove(next_sqube);

            // add the next p with the current q to the list    
            var p_next = next_prime(p);

            if (p_next == q) // p and q must be distinct
                p_next = next_prime(p_next);

            var new_sqube = (p_next*p_next * q*q*q);
            squbes[new_sqube] = (p_next, q);

            // if it was the one with the maximum q, add the next q, with p=2 to the squbes dictionary
            if (q == max_q)
            {
                max_q = next_prime(max_q);
                new_sqube = 4 * max_q * max_q * max_q;
                squbes[new_sqube] = (2, max_q);
            }

            return (next_sqube, p, q);
        }

        /// <summary>
        /// Tests if n is always non-prime if any one of its digits is changed
        /// </summary>
        private bool IsPrimeProof(ulong n)
        {
            var s = n.ToString();

            // special case if n is even:
            // it can only be turned into a prime by changing the last digit to an odd number
            // (note: this optimization only improves performance marginally, 1 sec for 30 sec run time)
            var l = s.Length;

            if ((n % 2) == 0)
                foreach (var d in "1379".ToCharArray())
                {
                    var s2 = s.Substring(0, s.Length - 1) + d;
                    ulong n2 = ulong.Parse(s2);
                    if (mr.IsPrime(n2))
                        return false;
                }
            else
                for (int i = 0; i < l; i++)
                    foreach (var d in "0123456789".ToCharArray())
                        if (d != s[i])
                        {
                            var s2 = s.Substring(0, i) + d + s.Substring(i + 1);
                            var n2 = ulong.Parse(s2);
                            if (mr.IsPrime(n2))
                                return false;
                        }
            return true;
        }
        
    }
}
