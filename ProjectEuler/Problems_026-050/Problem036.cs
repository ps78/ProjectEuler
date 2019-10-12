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
    /// https://projecteuler.net/problem=36
    ///
    /// The decimal number, 585 = 10010010012 (binary), is palindromic in both bases.
    /// Find the sum of all numbers, less than one million, which are palindromic in base 10 and base 2.
    /// (Please note that the palindromic number, in either base, may not include leading zeros.)
    /// </summary>
    public class Problem036 : EulerProblemBase
    {
        public Problem036() : base(36, "Double-base palindromes", 1_000_000, 872187) { }

        public override bool Test() => Solve(600) == 1055; //1+3+5+7+9+33+99+313+585

        public override long Solve(long n)
        {
            var result = new List<long>();
            for (long i = 1; i < n; i += 2)
                if (i.IsPalindrom() && i.ToBase(2).IsPalindrom())
                    result.Add(i);

            return result.Sum();
        }
    }
}
