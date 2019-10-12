using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=14
    ///     
    /// The following iterative sequence is defined for the set of positive integers:
    /// 
    /// n → n/2 (n is even)
    /// n → 3n + 1 (n is odd)
    /// 
    /// Using the rule above and starting with 13, we generate the following sequence:
    /// 13 → 40 → 20 → 10 → 5 → 16 → 8 → 4 → 2 → 1
    /// It can be seen that this sequence (starting at 13 and finishing at 1) contains 10 terms. 
    /// Although it has not been proved yet (Collatz Problem), it is thought that all starting numbers finish at 1.
    /// 
    /// Which starting number, under one million, produces the longest chain?
    /// </summary>
    public class Problem014 : EulerProblemBase
    {
        public Problem014() : base(14, "Longest Collatz sequence", 1_000_000, 837799) { }

        public override bool Test() => Solve(14) == 9;

        private Dictionary<ulong, ulong> cache = new Dictionary<ulong, ulong>(); // key=start value, value=sequence length

        public override long Solve(long n)
        {
            cache.Clear();
            ulong maxSeq = 0;
            ulong winner = 0;
            
            for (ulong i = 1; i < (ulong)n; i++)
            {
                var currentSeqLen = GetSequenceLength(i);
                if (currentSeqLen > maxSeq)
                {
                    winner = i;
                    maxSeq = currentSeqLen;
                }
            }

            return (long)winner;
        }

        private ulong GetSequenceLength(ulong start)
        {
            ulong result = 1;
            ulong i = start;
            while (i != 1)
            {
                if (cache.ContainsKey(i))
                {
                    result += cache[i] - 1;
                    break;
                }

                if (i % 2 == 0)
                    i = i / 2;
                else
                    i = 3 * i + 1;

                result++;
            }

            cache.Add(start, result);

            return result;
        }
        
    }
}
