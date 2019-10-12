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
    /// https://projecteuler.net/problem=34
    ///
    /// 145 is a curious number, as 1! + 4! + 5! = 1 + 24 + 120 = 145.
    /// Find the sum of all numbers which are equal to the sum of the factorial of their digits.
    /// Note: as 1! = 1 and 2! = 2 are not sums they are not included.
    /// </summary>
    public class Problem034 : EulerProblemBase
    {
        public Problem034() : base(34, "Digit factorials", 0, 40730) { }

        private static ulong[] factorials = new ulong[] { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880 };
        private long UpperLimit;
        
        public override long Solve(long n)
        {
            UpperLimit = 50000;
            var results = new List<long>();            
            CheckNum(0, 0, ref results);            
            return results.Sum();
        }

        private void CheckNum(long n, long sumFact, ref List<long> results)
        {
            if ((n >= 10) && (n == sumFact))
                results.Add(n);

            if (n > UpperLimit)
                return;

            int start = (n == 0 ? 1 : 0);
            for (int i = start; i <= 9; i++)
                CheckNum(long.Parse(n.ToString() + i.ToString()), sumFact + (long)factorials[i], ref results);
        }
    }
}
