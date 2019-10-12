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
    /// https://projecteuler.net/problem=32
    /// 
    /// We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once; for example, the 5-digit number, 15234, is 1 through 5 pandigital.
    ///
    ///  The product 7254 is unusual, as the identity, 39 × 186 = 7254, containing multiplicand, multiplier, and product is 1 through 9 pandigital.
    ///
    ///  Find the sum of all products whose multiplicand/multiplier/product identity can be written as a 1 through 9 pandigital.
    ///
    ///  HINT: Some products can be obtained in more than one way so be sure to only include it once in your sum.
    /// </summary>
    public class Problem032 : EulerProblemBase
    {
        public Problem032() : base(32, "Pandigital products", 0, 45228) { }

        public override long Solve(long n)
        {

            var p9 = GeneratePermutations("123456789", 4);
            /*
            var p8 = GeneratePermutations("12345678", 4);
            var p7 = GeneratePermutations("1234567", 3);
            var p6 = GeneratePermutations("123456", 3);
            var p5 = GeneratePermutations("12345", 2);
            var p4 = GeneratePermutations("1234", 2);
            */

            var result = new List<Tuple<int, int, int>>();
            result.AddRange(GetFactors(p9, 2, 3));
            result.AddRange(GetFactors(p9, 1, 4));
            /*
            result.AddRange(GetFactors(p8, 2, 2));
            result.AddRange(GetFactors(p8, 1, 3));
            result.AddRange(GetFactors(p7, 2, 2));
            result.AddRange(GetFactors(p7, 1, 3));
            result.AddRange(GetFactors(p6, 1, 2));
            result.AddRange(GetFactors(p5, 1, 2));
            result.AddRange(GetFactors(p4, 1, 1));
            */
            /*
            foreach (var x in result)
                Console.WriteLine("{0} = {1} x {2}", x.Item1, x.Item2, x.Item3);
            */
            return result.Select(x => x.Item1).Distinct().Sum();
        }

        private Tuple<int[], string[]> GeneratePermutations(string digits, int length)
        {
            int n = 1;
            for (int i = digits.Length; i > digits.Length - length; i--)
                n *= i;

            var result = new int[n];
            var unused = new string[n];
            int idx = 0;
            RecursivePermutations(digits, length, "", ref result, ref unused, ref idx);
            return new Tuple<int[], string[]>(result, unused);
        }

        private IEnumerable<Tuple<int, int, int>> GetFactors(Tuple<int[], string[]> input, int factorLen1, int factorLen2)
        {
            int n = input.Item1.Length;
            for (int i = 0; i < n; i++)
            {
                var perms = GeneratePermutations(input.Item2[i], factorLen1);
                for (int j = 0; j < perms.Item1.Length; j++)
                {
                    if ((input.Item1[i] % perms.Item1[j]) == 0)
                    {
                        int div = input.Item1[i] / perms.Item1[j];
                        string divStr = div.ToString();
                        if ((divStr.Length == factorLen2) && SameDigits(div.ToString(), perms.Item2[j]))
                            yield return new Tuple<int, int, int>(input.Item1[i], perms.Item1[j], div);
                    }
                }
            }
        }

        private bool SameDigits(string s1, string s2)
        {
            return s1.ToArray().Intersect(s2.ToArray()).Count() == s1.Length;
        }

        private void RecursivePermutations(string digitsLeft, int length, string currentValue, ref int[] state, ref string[] unused, ref int nextIndex)
        {
            if (currentValue.Length == length)
            {
                unused[nextIndex] = digitsLeft;
                state[nextIndex++] = int.Parse(currentValue);
            }
            else
            {
                for (int i = 0; i < digitsLeft.Length; i++)
                {
                    string dig = digitsLeft.Substring(0, i) + digitsLeft.Substring(i + 1);
                    RecursivePermutations(dig, length, currentValue + digitsLeft[i], ref state, ref unused, ref nextIndex);
                }
            }
        }
    }
}
