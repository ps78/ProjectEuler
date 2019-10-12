using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=146
    /// 
    /// The smallest positive integer n for which the numbers n2+1, n2+3, n2+7, n2+9, n2+13, and n2+27 are consecutive primes is 10. 
    /// The sum of all such integers n below one-million is 1242490.
    /// What is the sum of all such integers n below 150 million?
    /// 
    /// Solution optimized with http://www.mathblog.dk/project-euler-146-investigating-a-prime-pattern/
    /// </summary>
    public class Problem146 : EulerProblemBase
    {
        public Problem146() : base(146, "Investigating a Prime Pattern", 150_000_000, 676333270) { }

        public override bool Test() => Solve(1_000_000) == 1242490;

        private SieveOfEratosthenes sieve;
        private MillerRabinTest mrt;
        private ulong[] primes;

        public static ulong[] Add =  { 1, 3, 7, 9, 13, 27 };
        public static ulong[] NotAdd = { 19, 21 };

        public override long Solve(long n)
        {
            int nParallel = 8;

            ulong primeLimit = 6000;
            sieve = new SieveOfEratosthenes(primeLimit);
            primes = sieve.GetPrimes().ToArray();
            mrt = new MillerRabinTest();

            Tuple<ulong, bool[]>[] mods = new Tuple<ulong, bool[]>[primes.Length];
            for (int i = 0; i < primes.Length; i++)
                mods[i] = new Tuple<ulong, bool[]>(primes[i], Mods(primes[i]));

            ulong[] sum = new ulong[nParallel];
            for (int i = 0; i < nParallel; i++)
                sum[i] = 0;

            ulong[] from, to;
            SplitRange(10, (ulong)n, nParallel, out from, out to);

            Parallel.For(0, nParallel, (nThread) =>
            {
                for (ulong i = from[nThread]; i < to[nThread]; i += 10)
                {
                    ulong sq = i * i;

                    bool ok = true;

                    for (int j = 0; ok && (j < primes.Length) && (i * i > primes[j]); j++)
                        ok = mods[j].Item2[i % mods[j].Item1];

                    for (int j = 0; ok && (j < Add.Length); j++)
                        ok = IsProbablePrime(sq + Add[j]);

                    for (int j = 0; ok && (j < Add.Length); j++)
                        ok = mrt.IsPrime(sq + Add[j]);

                    for (int j = 0; ok && (j < NotAdd.Length); j++)
                        ok = !mrt.IsPrime(sq + NotAdd[j]);

                    if (ok)
                        sum[nThread] += i;
                }
            });

            return (long)sum.Sum();
        }

        public bool IsProbablePrime(ulong n)
        {
            // this line is only for performance reasons
            if ((n % 2 == 0) || (n % 3 == 0) || (n % 5 == 0) || (n % 7 == 0) || (n % 11 == 0))            
                return false;

            ulong root = (ulong)Math.Sqrt(n);

            for (int i = 5; i < primes.Length; i++)
            {
                ulong p = primes[i];
                if ((n % p) == 0)
                    return false;
                if (p > root)
                    break;
            }

            return true;
        }

        public static bool[] Mods(ulong m)
        {            
            bool[] res = new bool[m];
            for (ulong i = 0; i < m; i++)
            {
                res[i] = true;
                for (ulong j = 0; j < (ulong)Add.Length; j++)
                {
                    if ((i * i + Add[j]) % m == 0)
                    {
                        res[i] = false;
                        break;
                    }
                }
            }

            return res;
        }
        
        static void SplitRange(ulong lowerInclusive, ulong upperExclusive, int nPartitions, out ulong[] from, out ulong[] to)
        {
            from = new ulong[nPartitions];
            to = new ulong[nPartitions];

            ulong range = (upperExclusive - lowerInclusive) / (ulong)nPartitions;
            range = (range / 10) * 10; // make the range a multiple of 10
            ulong current = lowerInclusive;

            for (var i = 0; i < nPartitions; i++)
            {
                from[i] = current;
                to[i] = current + range;
                current += range;
            }
            to[nPartitions - 1] = upperExclusive;
        }
        
    }
}
