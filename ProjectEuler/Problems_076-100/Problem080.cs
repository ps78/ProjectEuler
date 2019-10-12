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
    /// https://projecteuler.net/problem=80
    /// It is well known that if the square root of a natural number is not an integer, then it is irrational. 
    /// The decimal expansion of such square roots is infinite without any repeating pattern at all.
    /// 
    /// The square root of two is 1.41421356237309504880..., and the digital sum of the first one hundred decimal digits is 475.
    /// For the first one hundred natural numbers, find the total of the digital sums of the first 
    /// one hundred decimal digits for all the irrational square roots.
    /// </summary>
    public class Problem080 : EulerProblemBase
    {
        public Problem080() : base(80, "Square root digital expansion", 0, 40886) { }

        public override long Solve(long n)
        {
            return Enumerable.Range(1, 100)
                            .Except(Enumerable.Range(1, 10)
                            .Select(x => x * x))
                            .Select(x => SquareRootDigitalSum(x, 100))
                            .Sum();
        }

        /// <summary>
        /// Calculate the square root using the digit-by-digit algorithm described here: https://en.wikipedia.org/wiki/Methods_of_computing_square_roots
        /// </summary>
        /// <param name="n"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public int SquareRootDigitalSum(int n, int digits)
        {
            string nStr = n.ToString();
            if (nStr.ToString().Length % 2 != 0)
                nStr = "0" + nStr;

            int decimalPos = nStr.Length;

            // result vector, initialize first digits with n, rest is 0
            var input = new byte[nStr.Length + 2*digits];            
            for (int i = 0; i < nStr.Length; i++)
                input[i] = byte.Parse(nStr[i].ToString());

            int idx = 0;
            BigInteger r = 0; // p = remainder
            BigInteger p = 0;
            while (idx < input.Length)
            {
                // c = current value
                BigInteger c = r * 100 + input[idx] * 10 + input[idx + 1];

                // find x such that x(20*p+x) <= c
                int x = 9;
                while (x * (20 * p + x) > c) x--;

                BigInteger y = x * (20 * p + x);

                p = p * 10 + x;
                r = c - y;
                
                idx += 2;
            }

            return p.ToString().ToArray().Take(digits).Select(d => int.Parse(d.ToString())).Sum();

            //return p.ToString().Insert(decimalPos / 2, ".");
        }
    }
}
