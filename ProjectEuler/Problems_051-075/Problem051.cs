using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=51
    /// By replacing the 1st digit of the 2-digit number *3, it turns out that six of the 
    /// nine possible values: 13, 23, 43, 53, 73, and 83, are all prime.
    /// By replacing the 3rd and 4th digits of 56**3 with the same digit, this 5-digit number 
    /// is the first example having seven primes among the ten generated numbers, yielding the 
    /// family: 56003, 56113, 56333, 56443, 56663, 56773, and 56993. 
    /// Consequently 56003, being the first member of this family, is the smallest prime with this property.
    /// Find the smallest prime which, by replacing part of the number (not necessarily 
    /// adjacent digits) with the same digit, is part of an eight prime value family.
    /// </summary>
    public class Problem051 : EulerProblemBase
    {
        public Problem051() : base(51, "Prime digit replacements", 0, 121313) { }
        private SieveOfEratosthenes sieve;

        public override long Solve(long n)
        {
            sieve = new SieveOfEratosthenes(1000000);

            var spos = new List<byte[]>(new byte[][] {
                new byte[] { 0, 1, 2 },
                new byte[] { 0, 1, 3 },
                new byte[] { 0, 2, 3 },
                new byte[] { 1, 2, 3 },
                new byte[] { 0, 1, 4 },
                new byte[] { 0, 2, 4 },
                new byte[] { 0, 3, 4 },
                new byte[] { 1, 2, 4 },
                new byte[] { 1, 3, 4 },
                new byte[] { 2, 3, 4 },
            });

            foreach (var p in sieve.GetPrimes(100000, sieve.Limit))
                foreach (var pos in spos)
                {
                    var sol = Get8PrimeFamily(p, pos);
                    if (sol != null)
                        return (long)sol[0];
                }

            return 0; 
        }

        private ulong[] Get8PrimeFamily(ulong p, byte[] substPos)
        {
            var s = new StringBuilder(p.ToString());
            int noPrimeCounter = 0;
            int k = 0;
            var solution = new ulong[8];
            foreach (char ch in "0123456789")
            { 
                foreach (var pos in substPos)
                    s[pos] = ch;
                ulong candidate = ulong.Parse(s.ToString());
                if (candidate < 100000 || !sieve.IsPrime(candidate)) 
                    noPrimeCounter++;
                else 
                    solution[k++] = candidate;
                if (noPrimeCounter > 2)
                    return null;    
            }
            return solution;
        }

        private string GetMask(ulong p, byte[] pos)
        {
            var s = new StringBuilder(p.ToString());
            foreach (var ps in pos)
                s[ps] = '*';
            return s.ToString();
        }
    }
}
