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
    /// https://projecteuler.net/problem=74
    /// The number 145 is well known for the property that the sum of the factorial of its digits is equal to 145:
    /// 
    /// 1! + 4! + 5! = 1 + 24 + 120 = 145
    /// 
    /// Perhaps less well known is 169, in that it produces the longest chain of numbers that link back to 169; it turns out that there are only three such loops that exist:
    /// 
    /// 169 → 363601 → 1454 → 169
    /// 871 → 45361 → 871
    /// 872 → 45362 → 872
    /// 
    /// It is not difficult to prove that EVERY starting number will eventually get stuck in a loop.For example,
    /// 
    /// 69 → 363600 → 1454 → 169 → 363601 (→ 1454)
    /// 78 → 45360 → 871 → 45361 (→ 871)
    /// 540 → 145 (→ 145)
    /// 
    /// Starting with 69 produces a chain of five non-repeating terms, but the longest non-repeating chain with a starting number below one million is sixty terms.
    /// 
    /// How many chains, with a starting number below one million, contain exactly sixty non-repeating terms?
    /// </summary>
    public class Problem074 : EulerProblemBase
    {
        public Problem074() : base(74, "Digit factorial chains", 1_000_000, 402) { }

        // factorials n! for n=0..9
        private int[] fact = new int[] { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880 };
        private int[] permuations = new int[] { };

        public override long Solve(long n)
        {
            // parallel-brute force:
            const int threadCount = 8;
            int maxN = (int)ProblemSize;
            int[] count = new int[threadCount];
            int batchSize = (int)Math.Ceiling((double)maxN / threadCount);            

            Parallel.For(0, threadCount, (p) =>
            {
                count[p] = 0;
                int ubound = Math.Min(maxN, (p + 1) * batchSize);
                for (int m = p * batchSize ; m < ubound; m++)
                    if (GetNonRepeatingChainLength(m) == 60)
                        count[p]++;
            });
            
            return count.Sum();                        
        }
        
        public int GetNonRepeatingChainLength(int n)
        {
            var set = new HashSet<int>(); 

            int i = 1;
            while(!set.Contains(n))
            {
                set.Add(n);                
                n = Next(n);
                i++;
            }
            return i - 1;
        }

        /// <summary>
        /// Computes the next element in the series of factorial. I.e. computes the sum of the factorials of the individual digits
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int Next(int n)
        {
            int sum = 0;            
            while (n > 0)
            {
                sum += fact[n % 10];
                n /= 10;
            } 
            return sum;
        }
    }
}
