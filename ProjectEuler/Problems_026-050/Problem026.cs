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
    /// https://projecteuler.net/problem=26
    /// 
    /// A unit fraction contains 1 in the numerator. The decimal representation of the unit fractions with denominators 2 to 10 are given:
    /// 1/2	= 	0.5
    /// 1/3	= 	0.(3)
    /// 1/4	= 	0.25
    /// 1/5	= 	0.2
    /// 1/6	= 	0.1(6)
    /// 1/7	= 	0.(142857)
    /// 1/8	= 	0.125
    /// 1/9	= 	0.(1)
    /// 1/10	= 	0.1
    /// Where 0.1(6) means 0.166666..., and has a 1-digit recurring cycle.It can be seen that 1/7 has a 6-digit recurring cycle.
    /// 
    // Find the value of d< 1000 for which 1/d contains the longest recurring cycle in its decimal fraction part.
    /// </summary>
    public class Problem026 : EulerProblemBase
    {
        public Problem026() : base(26, "Reciprocal cycles", 1000, 983) { }

        public override bool Test() => Solve(11) == 7;

        public override long Solve(long n)
        {
            int maxPeriod = 0;
            int maxDenom = 0;
            
            foreach (ulong prime in new SieveOfEratosthenes((ulong)n).GetPrimes(2, (ulong)n-1))
            {
                int denom = (int)prime;
                int rest = 1;
                for (int i = 0; i < denom; i++)
                    rest = (10 * rest) % denom;
                int r0 = rest;
                int period = 0;
                do
                {
                    rest = (10 * rest) % denom;
                    period++;
                } while (rest != r0);

                if (period > maxPeriod)
                {
                    maxPeriod = period;
                    maxDenom = denom;
                }                
            }

            return maxDenom;
        }

        /* initial solution: brute-force-like, runs in ~300ms
        
        private const int maxDecimals = 2000;

        public override ulong Solve()
        {
            int maxPeriod = 0;
            int denominator = 0;

            var sieve = new SieveOfEratosthenes(ProblemSize);

            foreach (long prime in sieve.GetPrimes())
            {
                int i = (int)prime;
                string result = LongDivison.Divide(1, i, maxDecimals + 1);
                result = result.Substring(0, result.Length - 1); // remove last digit to prevent rounding issues

                int period = GetPeriod(result);
                if (period > maxPeriod)
                {
                    maxPeriod = period;
                    denominator = i;
                }
            }

            //Console.WriteLine("max period {0} for 1/{1}", maxPeriod, denominator);
            //string s = LongDivison.Divide(1, denominator, maxDecimals);
            //File.WriteAllText(@"c:\temp\output.txt", s);            

            return (ulong)denominator;
        }

        private int GetPeriod(string val)
        {
            // take the decimal part, revert it
            string s = val.Substring(val.IndexOf('.') + 1);
            if (s.Length < maxDecimals)
                return 0;
            else
                s = new string(s.Reverse().ToArray());
            
            for (int p = 1; p < maxDecimals/2; p++)
                if (CheckPeriod(s, p))
                    return p;
            
            return 0;
        }

        private bool CheckPeriod(string s, int period)
        {
            for (int i = 0; i < period; i++)
                if (s[i] != s[period + i])
                    return false;
            return true;
        }
        */
    }
}
