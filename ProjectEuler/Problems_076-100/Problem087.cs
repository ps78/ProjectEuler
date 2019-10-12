using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;
using System.Diagnostics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=87
    /// The smallest number expressible as the sum of a prime square, prime cube, and prime fourth power is 28.
    /// In fact, there are exactly four numbers below fifty that can be expressed in such a way:
    /// 
    /// 28 = 22 + 23 + 24
    /// 33 = 32 + 23 + 24
    /// 49 = 52 + 23 + 24
    /// 47 = 22 + 33 + 24
    /// 
    /// How many numbers below fifty million can be expressed as the sum of a prime square, prime cube, and prime fourth power?
    /// </summary>
    public class Problem087 : EulerProblemBase
    {
        public Problem087() : base(87, "Prime power triples", 50_000_000, 1097343) { }

        public override bool Test() => Solve(50) == 4;

        public override long Solve(long n)
        {
            int N = (int)n;
            var sieve = new SieveOfEratosthenes((ulong)Math.Sqrt(N) + 1);
            var primes = sieve.GetPrimes();

            var squares = new List<int>();
            var cubes = new List<int>();
            var fourth = new List<int>();
            foreach (var p in primes)
            {
                if (p * p < (ulong)N)
                    squares.Add((int)(p * p));
                else break;
                if (p * p * p < (ulong)N)
                    cubes.Add((int)(p * p * p));
                if (p * p * p * p < (ulong)N)
                    fourth.Add((int)(p * p * p * p));
            }
            
            var numbers = new HashSet<int>();
            foreach (int s in squares)
                foreach (int c in cubes)
                    foreach (int f in fourth)
                    {
                        int k = s + c + f;
                        if (k < N && !numbers.Contains(k))
                            numbers.Add(k);
                    }

            return numbers.Count;
        }
    }
}
