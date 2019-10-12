using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using SudokuGame;

namespace ProjectEuler
{
    /// <summary>
    /// Solves https://projecteuler.net/problem=094
    /// It is easily proved that no equilateral triangle exists with integral length sides and integral area.
    /// However, the almost equilateral triangle 5-5-6 has an area of 12 square units.
    /// 
    /// We shall define an almost equilateral triangle to be a triangle for which two sides are equal 
    /// and the third differs by no more than one unit.
    /// 
    /// Find the sum of the perimeters of all almost equilateral triangles with integral side lengths and 
    /// area and whose perimeters do not exceed one billion (1,000,000,000).
    /// </summary>
    public class Problem094 : EulerProblemBase
    {
        public Problem094() : base(94, "Almost equilateral triangles", 1_000_000_000, 518408346) { }

        public override long Solve(long n)
        {
            int nParallel = 8;

            var solutions = new List<Tuple<long, long>>[8];

            // parallelize everything, compute boundaries
            long batchSize = (long)Math.Ceiling((n / 3.0) / nParallel);
            var lBound = new long[8];
            var uBound = new long[8];

            for (int p = 0; p < nParallel; p++)
            {
                lBound[p] = p == 0 ? 3 : uBound[p - 1] + 2;
                uBound[p] = lBound[p] + (batchSize % 2 == 0 ? batchSize : batchSize + 1);

                if (p == nParallel - 1)
                    uBound[p] = (int)(n / 3);
            }

            // let n be the length of the two sides with identical length, and m = n+/-1 the third length
            // then A = q sqrt( (n-q)(n+q) ) with q = (n-1)/2 or q = (n+1)/2
            // => n must be odd and (n-q)(n+q) must be a square number
            //for (ulong n = 3; n <= (ulong)(ProblemSize / 3); n += 2)
            Parallel.For(0, nParallel, (p) =>
            {
                solutions[p] = new List<Tuple<long, long>>();
                for (long m = lBound[p]; m < uBound[p]; m += 2)
                {
                    if (m > 1 && 3 * m < (long)n)
                    {
                        long q1 = (m + 1) / 2;
                        if (IsSquare((m - q1) * (m + q1)))
                            solutions[p].Add(new Tuple<long, long>(m, m + 1));

                        long q2 = (m - 1) / 2;
                        if (IsSquare((m - q2) * (m + q2)))
                            solutions[p].Add(new Tuple<long, long>(m, m - 1));
                    }
                }
            });

            return solutions.ToList().Sum(lst => lst.Select(x => 2 * x.Item1 + x.Item2).Sum());            
        }

        private bool IsSquare(long n)
        {
            // nice idea, but makes the whole thing slower:
            // squares cannot end in 2, 3, 7 or 8
            //ulong lastDigit = n % 10;
            //if (lastDigit == 2 || lastDigit == 3 || lastDigit == 7 || lastDigit == 8)
            //    return false;
            var sqrt = (long)Math.Sqrt(n);
            return sqrt * sqrt == n;
        }


        /// <summary>
        /// using binary search: much slower than floating point version
        /// </summary>
        private bool IsSquareBinarySearch(ulong n)
        {
            bool BinarySearch(ulong low, ulong high)
            {
                ulong mid = (low + high) / 2;
                ulong midSq = mid * mid;
                if (low > high)
                    return false;
                else if (n == midSq)
                    return true;
                else if (n < midSq)
                    return BinarySearch(low, mid - 1);
                else
                    return BinarySearch(mid + 1, high);
            }

            return BinarySearch(1, n);
        }        
    }
}
