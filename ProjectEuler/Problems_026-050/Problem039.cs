using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    using Triple = Tuple<int, int, int>;

    /// <summary>
    /// https://projecteuler.net/problem=39
    /// If p is the perimeter of a right angle triangle with integral length sides, {a,b,c}, there are exactly three solutions for p = 120.
    /// {20,48,52}, {24,45,51}, {30,40,50}
    /// For which value of p ≤ 1000, is the number of solutions maximised?
    /// </summary>
    public class Problem039 : EulerProblemBase
    {
        public Problem039() : base(39, "Integer right triangles", 1000, 840) { }

        public override bool Test() => Solve(121) == 120;

        public override long Solve(long n)
        {
            var triples = GenerateTriples((int)n).ToList();
            var counts = new List<int>(new int[n+1]);
            foreach (var t in triples)
                counts[t.Item1 + t.Item2 + t.Item3]++;

            int max = counts.Max();
            return counts.FindIndex(c => c == max);
        }

        /// <summary>
        /// Euclid's formula[1] is a fundamental formula for generating Pythagorean triples given an arbitrary 
        /// pair of positive integers m and n with m > n. The formula states that the integers
        /// a = m^2 - n^2 , b = 2mn, c = m^2 + n^2 
        /// form a Pythagorean triple
        /// </summary>
        private IEnumerable<Triple> GenerateTriples(int maxSum)
        {
            int n = 1;
            while (true)
            {
                int m = n + 1;
                if (((m - n) % 2) == 0)
                    m++;
                bool abort = true;
                while (true)
                {
                    int a = m * m - n * n;
                    int b = 2 * m * n;
                    int c = m * m + n * n;
                    int sum = a + b + c;

                    if (sum > maxSum)
                        break;

                    abort = false;
                    for (int k = 1; k <= maxSum / sum; k++)
                        yield return new Triple(k * a, k * b, k * c);
                    m += 2;
                }
                if (abort)
                    break;
                n++;
            }
        }
    }
}
