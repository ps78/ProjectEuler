using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=4
    ///
    /// A palindromic number reads the same both ways.The largest palindrome made from the product of two 2-digit numbers is 9009 = 91 × 99.
    /// Find the largest palindrome made from the product of two 3-digit numbers.
    /// 
    /// </summary>
    public class Problem004 : EulerProblemBase
    {
        public Problem004() : base(4, "Largest palindrome product", 3, 906609) { }

        public override bool Test() => Solve(2) == 9009;

        public override long Solve(long n)
        {
            var palindromes = new List<long>();

            long lowerLimit = (long)Math.Pow(10, n - 1);
            long upperLimit = (long)Math.Pow(10, n) - 1;

            for (long f1 = lowerLimit; f1 <= upperLimit; f1++)
                for (long f2 = f1; f2 <= upperLimit; f2++)
                {
                    long product = f1 * f2;
                    if (product.IsPalindrom())
                        palindromes.Add(product);
                }

            return palindromes.Max();
        }

    }
}
