using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=40
    /// An irrational decimal fraction is created by concatenating the positive integers:
    /// 0.123456789101112131415161718192021...
    /// It can be seen that the 12th digit of the fractional part is 1.
    /// If dn represents the nth digit of the fractional part, find the value of the following expression.
    /// d1 × d10 × d100 × d1000 × d10000 × d100000 × d1000000
    /// </summary>
    public class Problem040 : EulerProblemBase
    {
        public Problem040() : base(40, "Champernowne's constant", 0, 210) { }

        // define the positions of 10^i, for i = 0..10, expPos[i+1] = expPos[i] + i*(10^i - 10^(i-1))
        private static long[] expPos = new long[] { 1, 10, 190, 2890, 38890, 488890, 5888890, 68888890, 788888890, 8888888890, 98888888890 };
        
        public override long Solve(long n)
        {
            var digitPos = new long[] { 1, 10, 100, 1000, 10000, 100000, 1000000 };

            /* Brute fore approach:
            var sb = new StringBuilder(digitPos.Max());
            int i = 1;
            while (sb.Length < digitPos.Max())
                sb.Append((i++).ToString());
            
            return digitPos.ToList().Select(p => ulong.Parse(sb[(int)p-1].ToString())).Aggregate((d, s) => s *= d);
            */

            // intelligent approach:
            return digitPos.Select(p => GetDigit(p)).Aggregate((d, s) => s *= d);            
        }

        private long GetDigit(long pos)
        {
            if (pos < 10) return pos;
            long basePos = expPos.Where(e => e < pos).Max();
            long posLog = (long)Math.Log10(basePos);
            long num = (long)Math.Pow(10, posLog) + (pos - basePos) / (posLog + 1);
            return long.Parse(num.ToString().Substring((int)((pos - basePos) % (posLog + 1)), 1));
        }
    }
}
