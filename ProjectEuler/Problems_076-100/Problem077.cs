using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=77
    /// It is possible to write ten as the sum of primes in exactly five different ways:
    /// 
    /// 7 + 3
    /// 5 + 5
    /// 5 + 3 + 2
    /// 3 + 3 + 2 + 2
    /// 2 + 2 + 2 + 2 + 2
    /// 
    /// What is the first value which can be written as the sum of primes in over five thousand different ways?
    /// </summary>
    public class Problem077 : EulerProblemBase
    {
        public Problem077() : base(77, "Prime summations", 5000, 71) { }

        private SieveOfEratosthenes sieve;
        private List<ulong> primes;

        // first dic: 
        //   key = N, the number, for which partition counts are stored
        //   value = the partition counts for the key
        // second dic:
        //   key = all prime numbers smaller or equal to N 
        //   value = number of partitions of N that start with a value equal to key
        //
        // e.g. 11 has these prime paritions:
        //  11
        //  7 2 2
        //  5 3 3 
        //  5 2 2 2
        //  3 3 3 2
        //  3 2 2 2 2
        // hence the PartCount[11] contains this dictionary:
        //   (11, 1)
        //   (7, 1)
        //   (5, 2)
        //   (3, 2)
        //   (2, 0)
        private Dictionary<ulong, Dictionary<ulong, ulong>> PartCount = new Dictionary<ulong, Dictionary<ulong, ulong>>();

        public override long Solve(long n)
        {
            ulong limit = 1000;
            sieve = new SieveOfEratosthenes(limit);
            primes = sieve.GetPrimes();

            // initialize part counts for 2 and 3
            PartCount.Add(2, CreatePartCountDic(2));
            PartCount[2][2] = 1;
            PartCount.Add(3, CreatePartCountDic(3));
            PartCount[3][3] = 1;
            PartCount[3][2] = 0;

            ulong m = 4;
            while (true)
            {
                var dic = CreatePartCountDic(m);
                ulong p = FirstPrime(m);
                ulong d = m - p;
                
                while (p >= 2)
                {
                    if (d == 0)
                        dic[p] += 1;
                    else
                        dic[p] += PartCount[d].Where(kv => kv.Key <= p).Select(kv => kv.Value).Sum();

                    p = NextSmallerPrime(p);
                    d = m - p;
                }

                PartCount.Add(m, dic);

                ulong totalCount = (ulong)dic.Sum(kv => (long)kv.Value);
                //Console.WriteLine("Partition count of {0}: {1}", n, totalCount);
                if (totalCount > (ulong)n)
                    break;
                m++;
            }
            return (long)m;
        }
        
        // if n is a prime, return n, otherwise the next smallest prime that is at least 2 smaller than n
        private ulong FirstPrime(ulong n)
        {
            if (n < 4)
                return 0;
            else
                return sieve.IsPrime(n) ? n : primes.Where(x => x <= n - 2).Max();
        }

        private ulong NextSmallerPrime(ulong n)
        {
            return n <= 2 ? 0 : primes.Where(x => x < n).Max();            
        }

        // creates an empty dictionary, with keys = all primes <= N - 2 and values 0. Also includes N if N is prime
        private Dictionary<ulong, ulong> CreatePartCountDic(ulong N)
        {
            var d = new Dictionary<ulong, ulong>();
            ulong limit = sieve.IsPrime(N) ? N : N - 2;
            foreach (ulong p in primes.Where(x => x <= limit))
                d.Add(p, 0);
            return d;
        }
    }
}