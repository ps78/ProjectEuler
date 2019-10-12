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
    /// https://projecteuler.net/problem=30
    /// 
    /// Surprisingly there are only three numbers that can be written as the sum of fourth powers of their digits:
    /// 1634 = 14 + 64 + 34 + 44
    /// 8208 = 84 + 24 + 04 + 84
    /// 9474 = 94 + 44 + 74 + 44
    /// As 1 = 14 is not a sum it is not included.
    /// 
    /// The sum of these numbers is 1634 + 8208 + 9474 = 19316.
    /// 
    /// Find the sum of all the numbers that can be written as the sum of fifth powers of their digits.
    /// </summary>
    public class Problem030 : EulerProblemBase
    {
        public Problem030() : base(30, "Digit fifth powers", 5, 443839) { }

        public override bool Test() => Solve(4) == 19316;

        public override long Solve(long n)
        {
            var powers = new Dictionary<char, int>();
            int exp = (int)n;
            for (int i = 0; i <= 9; i++)
                powers.Add(i.ToString()[0], (int)Math.Pow(i, exp));

            int ulim = GetUpperLimit(exp);
            List<int> result = new List<int>();
            for (int i = 2; i < ulim; i++)
            {
                int sum = i.ToString().ToArray().Sum((char c) => powers[c]);
                if (sum == i)
                    result.Add(i);
            }

            return result.Sum();
        }

        private int GetUpperLimit(int exp)
        {
            int d = (int)Math.Pow(9, exp);
            for (int i = 1; i < 10; i++)
            {
                int limit = i * d;
                if (i > limit.ToString().Length)
                    return (i - 1)*d;
            }
            return -1;       
        }
    }
}
