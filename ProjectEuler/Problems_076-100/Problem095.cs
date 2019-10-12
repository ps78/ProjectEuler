using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// Solves https://projecteuler.net/problem=095
    /// The proper divisors of a number are all the divisors excluding the number itself.For example, the proper 
    /// divisors of 28 are 1, 2, 4, 7, and 14. As the sum of these divisors is equal to 28, we call it a perfect number.
    /// 
    /// Interestingly the sum of the proper divisors of 220 is 284 and the sum of the proper divisors of 284 is 220, 
    /// forming a chain of two numbers. For this reason, 220 and 284 are called an amicable pair.
    /// 
    /// Perhaps less well known are longer chains.For example, starting with 12496, we form a chain of five numbers:
    /// 
    /// 12496 → 14288 → 15472 → 14536 → 14264 (→ 12496 → ...)
    /// 
    /// Since this chain returns to its starting point, it is called an amicable chain.
    /// 
    /// Find the smallest member of the longest amicable chain with no element exceeding one million.
    /// </summary>
    public class Problem095 : EulerProblemBase
    {
        public Problem095() : base(95, "Amicable chains", 1_000_000, 14316) { }

        public override bool Test() => Solve(16000) == 12496;

        public override long Solve(long n)
        {
            // build a sieve with all divisor sums: The index of the array corresponds to the number itself and the conent is 
            // the sum if it's divisors            
            var divsum = new int[n + 1];
            for (int divisor = 1; divisor <= n / 2; divisor++)
                for (int i = 2*divisor; i <= n ; i += divisor)
                    divsum[i] += divisor;

            var chains = new List<List<int>>();

            // find chains
            int j = 2;
            while (j <= n)
            {
                // we need to skip perfect numbers, as they form cycles of length 1
                if (j != 6 && j!= 28 && j!=496 && j!=8128)
                {
                    var chain = new List<int>(20);
                    int k = j;
                    while (true)
                    {
                        // if come across a number that is already in the chain (but not the first one), we found a 
                        // cyclic chain, but not the start of it. Skip, will find the real cyclic chain later again
                        if (chain.Contains(k))
                            break;

                        chain.Add(k);
                        k = divsum[k];
                        // if we exceed the threshold, abort, it's not a cyclic chain
                        if (k > n || k == 0)
                            break;

                        // also stop if we are back where we started, in which case we found a chain
                        if (k == j)
                        { 
                            chains.Add(chain);
                            break;
                        }                        
                    } 
                }
                j++;
            }

            int maxChainLength = chains.Select(ch => ch.Count).Max();
            var longestChain = chains.Where(ch => ch.Count == maxChainLength).First();

            return longestChain.Min();
         }

        /// <summary>
        /// too slow, takes ~1.8 seconds
        /// 
        /// see also: https://en.wikipedia.org/wiki/Aliquot_sequence
        /// </summary>
        /// <returns></returns>
        public long Solve_FirstAttemt(long n)
        {
            int N = (int)n;
            var sieve = new SieveOfEratosthenes((ulong)N);
            var primes = sieve.GetPrimes().ToArray();
            var chains = new List<Tuple<int, int>>(); // chain length / start value                        
            var factorSums = new int[N + 1];

            // pre-compute factor sums: >95% of runtime is spent calculating the factor-sums            
            Parallel.For(1, N + 1, (i) =>
            {
                if (!sieve.IsPrime((ulong)i))
                {
                    var factors = sieve.GetFactors((ulong)i, primes).ToArray();
                    factorSums[i] = (int)factors.Sum() - i;
                }
            });

            for (int k = 2; k <= N; k++)
            {
                int m = k;
                bool chainFound = false;
                HashSet<int> numbersInChain = new HashSet<int>();
                while (true)
                {
                    if (m > N || m < k || m <= 1 || numbersInChain.Contains(m))
                        break;

                    numbersInChain.Add(m);

                    m = factorSums[m];

                    if (m == k)
                    {
                        chainFound = true;
                        break;
                    }
                }

                if (chainFound && numbersInChain.Count > 2)
                    chains.Add(new Tuple<int, int>(numbersInChain.Count, k));
            }

            int maxLength = chains.Select(x => x.Item1).Max();
            return chains.Where(x => x.Item1 == maxLength).Select(y => y.Item2).Min();
        }
    }
}
